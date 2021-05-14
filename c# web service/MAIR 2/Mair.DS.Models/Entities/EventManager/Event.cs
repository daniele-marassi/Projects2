using Mair.DS.Models.Base;
using Mair.DS.Models.Entities.EventManager.Actions;
using Mair.DS.Models.Entities.EventManager.Conditions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.EventManager
{
    [Table("Events", Schema = "EventManager")]
    public class Event: EntityBase
    {
        public string Name { get; set; }

        public long ActionId { get; set; }

        public virtual Action Action { get; set; }

        public long ConditionId { get; set; }

        public virtual Condition Condition { get; set; }

    }
}
