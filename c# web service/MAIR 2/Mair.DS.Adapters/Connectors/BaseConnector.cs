using Mair.DS.Adapters.DataObjects;
using Mair.DS.Common.Loggers;
using Mair.DS.Models.Entities.Automation;
using System;
using System.Collections.Generic;

namespace Mair.DS.Adapters.Connectors
{
    public abstract class BaseConnector : IConnector, IDisposable
    {
        public ConnectorState State { get; set; }
        public abstract Node Node { get; set; }
        public abstract List<Tag> Tags { get; set; }
        public DataChangeEventHandler DataChangeEventHandler { get; set; }

        protected ILogger logger;

        public abstract string ReadTagValue(string address);

        public abstract List<Tag> ReadAllTagsInfos(bool readAllarms, bool readMessages);

        public abstract Connection Connect();

        public abstract string WriteTagValue(string address, string value);

        public abstract bool Disconnect();

        public abstract void Dispose();

        public abstract bool Subscribe(string address);

        public abstract bool Unsubscribe(string address);
    }
}
