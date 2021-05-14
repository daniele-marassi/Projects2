using Mair.DS.Models.Entities.Automation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DS.Models.Entities.Automation
{
    public class TagValue
    {
        public TagValue(){ }

        public TagValue(Tag tag, string value)
        {
            Tag = tag;
            Value = value;
        }

        public Tag Tag { get; set; }

        public string Value { get; set; }

    }
}
