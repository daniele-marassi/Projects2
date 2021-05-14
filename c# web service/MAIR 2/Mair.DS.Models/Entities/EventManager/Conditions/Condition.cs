using Mair.DS.Models.Base;
using Mair.DS.Models.Entities.EventManager.Conditions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Mair.DS.Models.Entities.EventManager.Conditions
{
    [Table("Conditions", Schema="EventManager")]
    public class Condition : Jsonable, ICondition
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ConditionType Type { get => (ConditionType)base.Type; set => base.Type = value; }

        [NotMapped]
        public object JsonObj { get => (ICondition)base.JsonObj; set => base.JsonObj = value; }
    }
}