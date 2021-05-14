using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Models.Entities.EventManager.Conditions
{
    public class SimpleTagCondition : Condition, ICondition
    {
        public Tag Tag { get; set; }
    }
}