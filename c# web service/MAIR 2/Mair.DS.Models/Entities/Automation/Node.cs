using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Automation
{
    [Table("Nodes", Schema = "Automation")]
    public class Node : BusinessBaseEntity
    {
        public string Driver { get; set; }

        public string ConnectionString { get; set; }
    }
}
