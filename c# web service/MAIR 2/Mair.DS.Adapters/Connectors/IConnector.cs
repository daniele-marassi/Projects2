using Mair.DS.Adapters.DataObjects;
using Mair.DS.Models.Entities.Automation;
using System;
using System.Collections.Generic;

namespace Mair.DS.Adapters.Connectors
{
    public interface IConnector
    {
        Node Node { get; set; }
        List<Tag> Tags { get; set; }
        ConnectorState State { get; set; }
        DataChangeEventHandler DataChangeEventHandler { get; set; }
        string ReadTagValue(string address);
        List<Tag> ReadAllTagsInfos(bool readAllarms, bool readMessages);
        string WriteTagValue(string address, string value);
        Connection Connect();
        bool Disconnect();
        bool Subscribe(string address);
        bool Unsubscribe(string address);
    }
}