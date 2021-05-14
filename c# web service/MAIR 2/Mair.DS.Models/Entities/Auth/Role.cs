using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Auth
{
    [Table("Roles", Schema = "auth")]
    public class Role : EntityBaseWithDates
    {
         
        public string Type { get; set; }
    }
}
