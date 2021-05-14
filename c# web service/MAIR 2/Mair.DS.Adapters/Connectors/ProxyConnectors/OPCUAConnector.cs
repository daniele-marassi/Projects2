using Mair.DS.Adapters.DataObjects;
using Mair.DS.Common;
using Mair.DS.Common.Loggers;
using Mair.DS.Models.Entities.Automation;
using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.Net;
using Node = Mair.DS.Models.Entities.Automation.Node;

namespace Mair.DS.Adapters.Connectors.ProxyConnectors
{
    public class OPCUAConnector : BaseConnector, IConnector
    {
        private static readonly object locker = new object();
        private Session session;
        private Subscription subscription;
        private ConfiguredEndpoint endpoint;
        private int sessionId;
        public override Node Node { get; set; }
        public override List<Tag> Tags { get; set; }

        private ApplicationConfiguration applicationConfiguration;

        public Session Session
        {
            get
            {
                lock (locker)
                {
                    if (session == null)
                    { 
                        sessionId++;

                        session = Session.Create(
                            applicationConfiguration,
                            endpoint,
                            true,
                            string.Format("{0}-{1}", Dns.GetHostName(), sessionId),
                            60000,
                            new UserIdentity(new AnonymousIdentityToken()),
                            null).Result;
                    }
                }
                return session;
            }
            set
            {
                lock (locker)
                {
                    session = value;
                }
            }
        }

        public OPCUAConnector(Node node, List<Tag> tags)
        {
            logger = Instances.GetInstance<ILogger>();
            logger.LogInfo("Inizio fase di inizializzazione");

            Node = node;
            Tags = tags;

            logger.LogInfo("Creo configurazioen per sessione con server OPC UA");

            // Configurazione generale applicazione
            applicationConfiguration = new ApplicationConfiguration();
            applicationConfiguration.ApplicationName = string.Format("Standard-{0}", Dns.GetHostName());
            applicationConfiguration.ApplicationType = ApplicationType.Client;
            applicationConfiguration.ApplicationUri = new Uri(string.Format("urn:{0}", Dns.GetHostName())).ToString();
            applicationConfiguration.ProductUri = new Uri("https://www.mair-research.com/en/").ToString();

            // Configurazione per la gestione dei certificati
            applicationConfiguration.SecurityConfiguration = new SecurityConfiguration();
            applicationConfiguration.SecurityConfiguration.ApplicationCertificate = new CertificateIdentifier();
            applicationConfiguration.SecurityConfiguration.ApplicationCertificate.SubjectName = applicationConfiguration.ApplicationName;
            applicationConfiguration.SecurityConfiguration.AutoAcceptUntrustedCertificates = true;

            // Configurazione delle restrizioni della connessione
            // Per il momento vanno bene le impostazioni de Default
            applicationConfiguration.TransportQuotas = new TransportQuotas();

            // Configurazione client
            // Per il momento vanno bene le impostazioni de Default
            applicationConfiguration.ClientConfiguration = new ClientConfiguration();

            // Valido la configurazione e la libreria imposta automaticamente delle variabili interne
            // sulla base del tipo dell'applicazione stessa
            applicationConfiguration.Validate(applicationConfiguration.ApplicationType);

            // Autoaccetta tutti i certificati non fidati (Untrusted)
            applicationConfiguration.CertificateValidator.CertificateValidation += AutoAcceptCertificate;

            logger.LogInfo("Fine creazione configurazione");

            Connect();

            Session.KeepAlive += Session_KeepAlive;
            Session.KeepAliveInterval = 60000;
            Session.SessionClosing += Session_SessionClosing;

            subscription = new Subscription(Session.DefaultSubscription) { PublishingInterval = 1000 };

            Session.AddSubscription(subscription);
            subscription.Create();

            logger.LogInfo("Finito la fase di inizializzazione");

        }

        public override Connection Connect()
        {
            logger.LogInfo("Tento la connessine al server OPC UA");

            Connection connection = new Connection();
            try
            {
                endpoint = new ConfiguredEndpoint(null,
                    CoreClientUtils.SelectEndpoint(Node.ConnectionString, false),
                    EndpointConfiguration.Create(applicationConfiguration));
                connection.OPCUASTDConnection = Session;
            }
            catch(Exception e)
            {
                logger.LogErr(e);
                Connect();
            }

            logger.LogInfo("Mi sono connesso e ho creato la sessione");

            State = ConnectorState.Connected;

            return connection;
        }

        public override bool Disconnect()
        {
            logger.LogInfo("Mi disconnetto");
            Session.Close();
            logger.LogInfo("Mi sono disconnesso");
            State = ConnectorState.Disconnected;

            return Session.Connected;
        }

        public override void Dispose()
        {
            State = ConnectorState.Connected;

            Session.Dispose();
        }

        public override string ReadTagValue(string address)
        {
            string value = ReadDataValue(address).ToString();
            return value;
        }

        public override List<Tag> ReadAllTagsInfos(bool readAllarms, bool readMessages)
        {
            List<Tag> tags = new List<Tag>();
            ReferenceDescription DBG = GetReferenceDescriptionByDisplayName("DataBlocksGlobal");
            var DBGCollection = Session.FetchReferences(ExpandedNodeId.ToNodeId(DBG.NodeId, Session.NamespaceUris));
            ReferenceDescriptionCollection referenceDescriptions = new ReferenceDescriptionCollection();

            foreach (var child in DBGCollection)
            {
                if (child.DisplayName.Text.StartsWith("T") || child.DisplayName.Text.StartsWith("Z"))
                {
                    Console.WriteLine("Leggo: " + child.NodeId);
                    referenceDescriptions.AddRange(GetAllReferenceDescription(child, Session, readAllarms, readMessages));
                }
            }

            foreach (var item in referenceDescriptions)
            {
                DataValue dataValue = ReadDataValue(item.NodeId.ToString());

                Tag tag = new Tag()
                {
                    Name = item.DisplayName.Text,
                    Description = null,
                    NodeId = Node.Id,
                    Address = item.NodeId.ToString(),
                    IsEnabled = false
                };

                if (dataValue.WrappedValue.TypeInfo != null)
                {
                    tag.Description = ReadDataValue(item.NodeId.ToString()).WrappedValue.TypeInfo.BuiltInType.ToString();
                    tag.IsEnabled = true;
                }

                tags.Add(tag);
            }

            return tags;
        }

        private List<ReferenceDescription> GetAllReferenceDescription(ReferenceDescription node, Session session, bool returnAllarms, bool returnMessages)
        {
            List<ReferenceDescription> referenceDescriptions = new List<ReferenceDescription>();
            if (node.NodeClass == NodeClass.Variable)
                if (!node.DisplayName.Text.StartsWith("ALL") && !node.DisplayName.Text.StartsWith("MSG"))
                    referenceDescriptions.Add(node);
                else
                    if ((node.DisplayName.Text.StartsWith("ALL") && returnAllarms) || (node.DisplayName.Text.StartsWith("MSG") && returnMessages))
                    referenceDescriptions.Add(node);


            var childs = session.FetchReferences(ExpandedNodeId.ToNodeId(node.NodeId, session.NamespaceUris));

            foreach (var child in childs)
            {
                if (child.NodeClass == NodeClass.Variable)
                    if (!child.DisplayName.Text.StartsWith("ALL") && !child.DisplayName.Text.StartsWith("MSG"))
                        referenceDescriptions.AddRange(GetAllReferenceDescription(child, session, returnAllarms, returnMessages));
                    else
                        if ((child.DisplayName.Text.StartsWith("ALL") && returnAllarms) || (child.DisplayName.Text.StartsWith("MSG") && returnMessages))
                        referenceDescriptions.AddRange(GetAllReferenceDescription(child, session, returnAllarms, returnMessages));
            }

            return referenceDescriptions;
        }

        private ReferenceDescription GetReferenceDescriptionByDisplayName(string displayName)
        {
            ReferenceDescription DBGReferenceDescription = null;
            ReferenceDescriptionCollection root = Session.FetchReferences(ObjectIds.ObjectsFolder);
            Byte[] continuationPoint;

            // GetByRecursion(string nodeIdDisplayNameText)
            // GetByCycle(string nodeIdDisplayNameText)
            do
            {
                for (int i = 0; i < root.Count; i++)
                {
                    var item = root[i];
                    if (item.DisplayName.Text == displayName)
                    {
                        DBGReferenceDescription = item;
                        break;
                    }
                    else
                    {
                        ReferenceDescriptionCollection referenceDescriptions;
                        Session.Browse(
                            null,
                            null,
                            ExpandedNodeId.ToNodeId(item.NodeId, Session.NamespaceUris),
                            0u,
                            BrowseDirection.Forward,
                            ReferenceTypeIds.HierarchicalReferences,
                            true,
                            (uint)NodeClass.Object,
                            out continuationPoint,
                            out referenceDescriptions);
                        root.AddRange(referenceDescriptions);

                        // Ottimizzazione?
                        root.RemoveAt(i);
                        i--;
                    }
                }

            } while (DBGReferenceDescription == null);

            return DBGReferenceDescription;
        }

        public override string WriteTagValue(string address, string value)
        {
            WriteValueCollection valuesToWrite = new WriteValueCollection();
            DataValue dataValue = ReadDataValue(address);
            object objectToWrite = TypeInfo.Cast(value, dataValue.WrappedValue.TypeInfo.BuiltInType);

            WriteValue valueToWrite = new WriteValue()
            {
                NodeId = ExpandedNodeId.ToNodeId(address, Session.NamespaceUris),
                Value = new DataValue()
                {
                    Value = objectToWrite
                },
                AttributeId = Attributes.Value
            };

            valuesToWrite.Add(valueToWrite);

            StatusCodeCollection statusCodes = null;
            DiagnosticInfoCollection diagnosticInfos = null;

            Session.Write(null, valuesToWrite, out statusCodes, out diagnosticInfos);

            return ReadTagValue(address);
        }

        private DataValue ReadDataValue(string address)
        {
            DataValue dataValue = new DataValue();
            try
            {
                dataValue = Session.ReadValue(ExpandedNodeId.ToNodeId(address, Session.NamespaceUris));
            }
            catch (Exception e)
            {
                logger.LogInfo(string.Format("Errore nella lettura: {0} - {1}", address, e.Message));
            }
            return dataValue;
        }

        private void AutoAcceptCertificate(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }

        private void Session_KeepAlive(Session session, KeepAliveEventArgs e)
        {
            logger.LogInfo(string.Format("Sessione: {0} {1} {2}", session.SessionId, session.SessionName, e.CurrentState));
            if (e.CurrentState != ServerState.Running)
            {
                State = ConnectorState.Reconnecting;
                logger.LogErr(string.Format("Sessione: {0} {1} Problema: {2}", session.SessionId, session.SessionName, e.Status.LocalizedText));
                logger.LogInfo("Tento la riconnessione creando una nuova sessione.");
                SessionReconnectHandler sessionReconnectHandler = new SessionReconnectHandler();
                sessionReconnectHandler.BeginReconnect(session, 1000, (sender, ev) =>
                {
                    Session = ((SessionReconnectHandler)sender).Session;
                    logger.LogInfo(string.Format("Nuova sessione: {0} aperta", Session.SessionId));
                    State = ConnectorState.Connected;
                });
            }
        }


        private void Session_SessionClosing(object sender, EventArgs e)
        {
            logger.LogInfo(string.Format("Sto chiudendo la sessione: {0} - {1}", ((Session)sender).SessionId, ((Session)sender).SessionName));
            State = ConnectorState.Disconnected;
        }

        public override bool Subscribe(string address)
        {
            bool subscribed = false;
            MonitoredItem monitoredItem = new MonitoredItem(subscription.DefaultItem);
            monitoredItem.StartNodeId = ExpandedNodeId.ToNodeId(address, Session.NamespaceUris);

            //DataValue dataNode = ReadDataValue(address);

            //if (dataNode != null)
            //{
                monitoredItem.Notification += MonitoredItem_Notification;
                subscription.AddItem(monitoredItem);
                subscription.ApplyChanges();
                subscribed = true;
            //}

            return subscribed;
        }

        public override bool Unsubscribe(string address)
        {
            bool unsubscribed = false;
            MonitoredItem monitoredItem;
            monitoredItem = ((List<MonitoredItem>)subscription.MonitoredItems).Find(mi => mi.StartNodeId.ToString() == address);
            if (monitoredItem != null)
            {
                subscription.RemoveItem(monitoredItem);
                subscription.ApplyChanges();
                unsubscribed = true;
            }

            return unsubscribed;
        }

        private void MonitoredItem_Notification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            foreach (var item in monitoredItem.DequeueValues())
            {
                logger.LogInfo(string.Format("tag: {0} = {1}", monitoredItem.StartNodeId, item.Value));

                DataChangeEventHandler(this, new DataChangeEventArgs(Tags.Find(t => t.Address == monitoredItem.StartNodeId.ToString()), item.Value.ToString()));
                //DataChangeEventHandler(this, Tags.Find(t => t.Address == monitoredItem.StartNodeId.ToString()) item.Value.ToString());
            }
        }
    }
}
