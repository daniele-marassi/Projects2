using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Automation
{
    [Table("SimulatedConnector", Schema = "Automation")]
    public class SimulatedConnector : EntityBaseWithDates
    {
        public long TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public string Value { get; set; }
    }
}
