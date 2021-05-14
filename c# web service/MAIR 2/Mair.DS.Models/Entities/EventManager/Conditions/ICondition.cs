using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DS.Models.Entities.EventManager.Conditions
{
    public interface ICondition
    {
        string Name { get; set; }
        string Description { get; set; }
        ConditionType Type { get; set; }
    }
}
