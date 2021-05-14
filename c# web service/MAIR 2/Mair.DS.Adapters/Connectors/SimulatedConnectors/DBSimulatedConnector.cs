using Mair.DS.Adapters.DataObjects;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Mair.DS.Models.Entities.Automation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DS.Adapters.Connectors.SimulatedConnectors
{
    public class DBSimulatedConnector : BaseConnector, IConnector
    {
        AutomationContext automationContext { get; set; }
        SimulatedConnectorRepository SimulatedConnectorRepository { get; set; }
        public override Node Node { get; set; }
        public override List<Tag> Tags { get; set; }

        public DBSimulatedConnector(Node node, List<Tag> tags, AutomationContext automationContext)
        {
            Node = node;
            Tags = tags;
            node.Driver= "DBSimulatedConnector";
            node.ConnectionString = automationContext.Database.GetDbConnection().ConnectionString;
            this.automationContext = automationContext;
        }

        public override string ReadTagValue(string address)
        {
            var tagId = Tags.Find(t => t.Address == address).Id;

            var simulatedValues = SimulatedConnectorRepository.Read();
            
            var simulatedValue = simulatedValues.Result.Find(sv => sv.TagId == tagId);

            return simulatedValue.Value;
        }

        public override string WriteTagValue(string address, string value)
        {
            var tagValues = SimulatedConnectorRepository.Read();

            var tagValue = tagValues.Result.Find(tv => tv.TagId == long.Parse(address));

            tagValue.Value = value;

            var dbSet = automationContext.Set<SimulatedConnector>();

            var ret = dbSet.Update(tagValue);

            automationContext.SaveChanges();

            return tagValue.Value;
        }

        public override Connection Connect()
        {
            SimulatedConnectorRepository = new SimulatedConnectorRepository(automationContext);

            var connection = new Connection();
            connection.DBSimulatedConnection = SimulatedConnectorRepository;

            return connection;
        }

        public override bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //VARIE LOGICHE DI CHIUSURA DELLE CONNESSIONI E DISTRUZIONE DEGLI OGGETTI
            throw new NotImplementedException();
        }

        public override List<Tag> ReadAllTagsInfos(bool readAllarms, bool readMessages)
        {
            return Tags;
        }

        public override bool Subscribe(string address)
        {
            return true;
            //throw new NotImplementedException();
        }

        public override bool Unsubscribe(string address)
        {

            throw new NotImplementedException();
        }
    }
}
