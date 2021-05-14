using Mair.DS.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mair.DS.Models.Entities.EventManager.Actions
{
    [Table("DbActionsDetails", Schema = "EventManager")]
    public class DbActionDetail : EntityBase
    {
        public long DbActionsId { get; set; }
        public virtual DbAction DbActions { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        // long.Parse perchè a db è una stringa...
        [NotMapped]
        public ReferenceType ReferenceType { get { return (ReferenceType)long.Parse(Type); } }

    }
}
