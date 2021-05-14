using Mair.DS.Models.Entities.Automation;
using System;

namespace Mair.DS.Adapters.Connectors
{
    public delegate void DataChangeEventHandler(object sender, DataChangeEventArgs e);

    public class DataChangeEventArgs : EventArgs
    {
        public DataChangeEventArgs(Tag tag, string value)
        {
            Tag = tag;
            Value = value;
        }

        public Tag Tag { get; set; }
        public string Value { get; set; }
    }
}
