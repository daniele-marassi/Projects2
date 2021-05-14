using Mair.DS.Models.Entities.Automation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DS.Models.Entities.EventManager.Conditions
{
    public class MairEventCondition : Condition, ICondition
    {
        public Tag Start { get; set; }
        public Tag End { get; set; }
        public Tag Ack { get; set; }

    }
}
