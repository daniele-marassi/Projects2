using Mair.DS.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.EventManager.Actions
{
    [Table("DbActions", Schema = "EventManager")]
    public class DbAction : Action
    {
        [NotMapped]
        public virtual List<DbActionDetail> DbActionDetails { get; set; }

    }
}
