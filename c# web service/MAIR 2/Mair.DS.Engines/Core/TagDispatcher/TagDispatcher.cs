using Mair.DS.Adapters.DataObjects;
using Mair.DS.Adapters.Connectors;
using Mair.DS.Adapters.Connectors.ProxyConnectors;
using Mair.DS.Adapters.Connectors.SimulatedConnectors;
using Mair.DS.Common;
using Mair.DS.Common.Loggers;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Mair.DS.Models.Entities.Automation;
using System;
using System.Collections.Generic;
using Mair.DS.Engines.Core.EventManager;
using Microsoft.EntityFrameworkCore;

namespace Mair.DS.Engines.TagDispatcher
{
    public class TagDispatcher : ITagDispatcher
    {
        private List<IConnector> connectors { get; set; }
        private AutomationContext automationContext { get; set; }
        private NodeRepository nodeRepository { get; set; }
        private TagRepository tagRepository { get; set; }
        private SimulatedConnectorRepository simulatedConnectorRepository { get; set; }
        public string Name { get; set; }
        private ILogger logger { get; set; }
        public EventManagerEventHandler Notifier { get; set; }

        public TagDispatcher()
        {
            logger = Instances.GetInstance<ILogger>();
            Name = "TagDispatcher";
        }

        public void Init()
        {
            logger.LogInfo("Inizio inizializzazione");
            connectors = new List<IConnector>();
            var optionBuilder = new DbContextOptionsBuilder<AutomationContext>();
            optionBuilder.UseSqlServer(Models.Defaults.ConnectionString);

            logger.LogInfo(string.Format("Mi connetto al DataBase: {0}", Models.Defaults.ConnectionString));

            automationContext = new AutomationContext(optionBuilder.Options);
            //automationContext = Instances.GetInstance<AutomationContext>();

            nodeRepository = new NodeRepository(automationContext);
            tagRepository = new TagRepository(automationContext);
            simulatedConnectorRepository = new SimulatedConnectorRepository(automationContext);

            logger.LogInfo("Carico in memoria i connettori");
            var nodes = nodeRepository.Read().Result;
            logger.LogInfo(string.Format("Caricati: {0} connettori", nodes.Count));

            logger.LogInfo("Carico le tag");
            var tags = tagRepository.Read().Result;
            logger.LogInfo(string.Format("Caricate: {0} tag", tags.Count));

            logger.LogInfo("Comincio ad istanziare i connettori");
            connectors = InstantiateConnectors(nodes, tags, Defaults.IsPlcSimulated);
            logger.LogInfo(string.Format("Istanziati: {0}", connectors.Count));

            logger.LogInfo("Comincio sottoscrizione a tutte le tag di tutti i conenttori");

            foreach (var tag in tags)
            {
                SubscribeDataChange(tag);
            }

            logger.LogInfo("Fine inizializzazione");
        }

        private List<IConnector> InstantiateConnectors(List<Node> nodes, List<Tag> tags, bool isPlcSimulated = false)
        {
            IConnector connector;
            List<IConnector> connectors = new List<IConnector>();

            foreach (var node in nodes)
            {
                var nodeTags = tags.FindAll(t => t.NodeId == node.Id && t.IsEnabled == true);
                if (isPlcSimulated)
                {
                    logger.LogInfo("Comincio ad istanziare connettori simulati");
                    connector = new DBSimulatedConnector(node, nodeTags, automationContext);
                }
                else
                    switch (node.Driver)
                    {
                        case "OPCUASTD":
                            logger.LogInfo(string.Format("Comincio ad istanziare un connettore di tipo: {0}", node.Driver));
                            connector = new OPCUAConnector(node, nodeTags);
                            connector.DataChangeEventHandler += DataChangeEventHandler;
                            break;
                        default:
                            logger.LogInfo(string.Format("Comincio ad istanziare un connettore di tipo: {0}", node.Driver));
                            connector = new OPCUAConnector(node, nodeTags);
                            break;
                    }
                logger.LogInfo("Connettore istanziato");
                connectors.Add(connector);
                logger.LogInfo("Connettore aggiunto alla lista connettori");
            }
            return connectors;
        }

        public int UpdateTagConfig()
        {
            logger.LogInfo("Comincio aggiornamento configurazione Tag e Nodi");
            List<Tag> tags = new List<Tag>();
            foreach (var connector in connectors)
            {
                logger.LogInfo(string.Format("Leggo tutte le tag di {0}", connector.Node));
                if (Defaults.IsPlcSimulated)
                    using (var connection = connector.Connect().DBSimulatedConnection)
                        tags = connector.ReadAllTagsInfos(false, false);
                else
                    switch (connector.Node.Driver)
                    {
                        case "OPCUASTD":
                            tags = connector.ReadAllTagsInfos(false, false);
                            break;
                        default:
                            tags = connector.ReadAllTagsInfos(false, false);
                            break;
                    }
            }
            int i = 0;
            foreach (var tag in tags)
            {
                foreach (var connector in connectors)
                {
                    if (!connector.Tags.Exists(t => t.NodeId == tag.NodeId && t.Address == tag.Address) && tag.IsEnabled)
                    {
                        logger.LogInfo(string.Format("Aggiungo tag: {0}", tag.Address));
                        Tag t = tagRepository.Create(tag);

                        connector.Tags.Add(t);

                        SimulatedConnector simulatedConnector = new SimulatedConnector()
                        {
                            TagId = t.Id,
                            Value = "0"
                        };
                        simulatedConnectorRepository.Create(simulatedConnector);
                        i++;
                        break;
                    }

                }
            }
            return i;
        }

        public TagValue ReadTagValue(Tag tag)
        {
            // Leggo la configurazione all'interno di Connector e poi leggo il valore all'indirizzo
            // di memoria della configurazione

            TagValue tagValue = new TagValue();
            foreach (var connector in connectors)
            {
                var tagToRead = connector.Tags.Find(t => t.Id == tag.Id);
                if (tagToRead != null)
                {
                    logger.LogInfo(string.Format("Leggo il valore della tag: {0}", tagToRead.Address));
                    if (Defaults.IsPlcSimulated)
                        using (var connection = connector.Connect().DBSimulatedConnection)
                            tagValue = ToTagValue(tag, connector.ReadTagValue(tagToRead.Address));
                    else
                        switch (connector.Node.Driver)
                        {
                            case "OPCUASTD":
                                tagValue = ToTagValue(tag, connector.ReadTagValue(tagToRead.Address));
                                break;
                            case "S7":
                            //TODO
                            //using (var connection = connector.Connect().S7Connection)
                            //    value = connector.Read(tag);
                            default:
                                tagValue = ToTagValue(tag, connector.ReadTagValue(tagToRead.Address));
                                break;
                        }
                    break;
                }
            }
            return tagValue;
        }

        public List<TagValue> ReadAllTagsValue()
        {
            logger.LogInfo("Leggo il valore di tutte le tag");
            List<TagValue> tagValueDOs = new List<TagValue>();
            foreach (var connector in connectors)
            {
                foreach (var tag in connector.Tags)
                {
                    tagValueDOs.Add(ToTagValue(tag, connector.ReadTagValue(tag.Address)));
                }
            }
            logger.LogInfo("Ho letto tutte le tag");
            return tagValueDOs;
        }

        public string WriteTagValue(Tag tag, string value)
        {
            value = value.Replace(',', '.');
            string valueW = null;
            foreach (var connector in connectors)
            {
                var tagToWrite = connector.Tags.Find(t => t.Id == tag.Id);
                if (tagToWrite != null)
                {
                    logger.LogInfo(string.Format("Scrivo il valore della tag: {0}", tagToWrite.Address));
                    if (Defaults.IsPlcSimulated)
                        using (var connection = connector.Connect().DBSimulatedConnection)
                            valueW = connector.WriteTagValue(tagToWrite.Id.ToString(), value);
                    else
                        switch (connector.Node.Driver)
                        {
                            case "OPCUASTD":
                                valueW = connector.WriteTagValue(tagToWrite.Address, value);
                                break;
                            case "S7":
                                //TODO
                                //using (var connection = connector.Connect().S7Connection)
                                //    valueW = connector.Write(tag, value);
                                break;
                            default:
                                valueW = connector.WriteTagValue(tagToWrite.Address, value);
                                break;
                        }
                    break;
                }
            }
            return valueW;
        }


        public bool SubscribeDataChange(Tag tag)
        {
            logger.LogInfo(string.Format("Comincio sottoscrizione per tag: {0}", tag.Address));

            bool subscribed = false;
            foreach (var connector in connectors)
            {
                var tagToSubscribe = connector.Tags.Find(t => t.Id == tag.Id);
                if (tagToSubscribe != null)
                {
                    logger.LogInfo(string.Format("Mi sottoscrivo alla tag: {0}", tagToSubscribe.Address));
                    if (Defaults.IsPlcSimulated)
                        using (var connection = connector.Connect().DBSimulatedConnection)
                            subscribed = connector.Subscribe(tagToSubscribe.Id.ToString());
                    else
                        switch (connector.Node.Driver)
                        {
                            case "OPCUASTD":
                                {
                                    subscribed = connector.Subscribe(tagToSubscribe.Address);
                                    logger.LogInfo(string.Format("Sottoscrizione completata alla tag: {0}", tag.Address));
                                }
                                break;
                            case "S7":
                                //TODO
                                //using (var connection = connector.Connect().S7Connection)
                                //    valueW = connector.Write(tag, value);
                                break;
                            default:
                                subscribed = connector.Subscribe(tagToSubscribe.Address);
                                break;
                        }
                    break;
                }
            }

            return subscribed;
        }

        public bool UnsubscribeDataChange(Tag tag)
        {
            bool unsubscribed = false;
            foreach (var connector in connectors)
            {
                var tagToUnsubscribe = connector.Tags.Find(t => t.Id == tag.Id);
                if (tagToUnsubscribe != null)
                {
                    logger.LogInfo(string.Format("Tolgo la sottoscrizione dalla tag: {0}", tagToUnsubscribe.Address));
                    if (Defaults.IsPlcSimulated)
                        using (var connection = connector.Connect().DBSimulatedConnection)
                            unsubscribed = connector.Unsubscribe(tagToUnsubscribe.Id.ToString());
                    else
                        switch (connector.Node.Driver)
                        {
                            case "OPCUASTD":
                                unsubscribed = connector.Unsubscribe(tagToUnsubscribe.Address);
                                break;
                            case "S7":
                                //TODO
                                //using (var connection = connector.Connect().S7Connection)
                                //    valueW = connector.Write(tag, value);
                                break;
                            default:
                                unsubscribed = connector.Unsubscribe(tagToUnsubscribe.Address);
                                break;
                        }
                    break;
                }
            }
            return unsubscribed;
        }

        private TagValue ToTagValue(Tag tag, string value)
        {
            TagValue tagValueDO = new TagValue(tag, value);
            return tagValueDO;
        }

        public void DataChangeEventHandler(object sender, DataChangeEventArgs dataChangeEventArgs)
        {
            TagValue tagValue = ToTagValue(dataChangeEventArgs.Tag, dataChangeEventArgs.Value);

            Notifier(this, tagValue);
        }
    }
}
