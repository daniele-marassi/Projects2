using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.EventManager.Actions
{
    [Table("Actions", Schema = "EventManager")]
    public class Action : Jsonable
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public ActionType ActionTypeId { get; set; }

        //[NotMapped]
        //public ActionType Type { get { return (ActionType)ActionTypeId; } }

        public long ActionId { get; set; }

        // Rimuovere?
        //[NotMapped]
        //public Actions.BaseAction ActionDetail { get; set; }
    }
}
