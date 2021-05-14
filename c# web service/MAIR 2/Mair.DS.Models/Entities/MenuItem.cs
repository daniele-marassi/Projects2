using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities
{
    [Table("MenuItems", Schema = "dbo")]
    public class MenuItem : BusinessBaseEntity
    {

        public long? ParentId { get; set; }

        public int Order { get; set; }

        public string Link { get; set; }

        public bool Enable { get; set; }

        public bool Visible { get; set; }
    }
}
