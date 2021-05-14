using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Automation
{
    [Table("Tags", Schema = "Automation")]
    public class Tag : BusinessBaseEntity
    {
        public string Address { get; set; }

        public bool IsEnabled { get; set; }

        public long NodeId { get; set; }

        public virtual Node Node { get; set; }
    }
}
