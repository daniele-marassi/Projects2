using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Auth
{
    [Table("RolePaths", Schema = "auth")]
    public class RolePath : BusinessBaseEntity
    {

        public long RoleId { get; set; }

        public string Path { get; set; }

        public bool IsEnabled { get; set; }
    }
}
