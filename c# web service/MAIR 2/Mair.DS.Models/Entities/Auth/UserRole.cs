using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Auth
{
    [Table("UserRoles", Schema = "auth")]
    public class UserRole : EntityBaseWithDates
    {
         
        public long UserId { get; set; }

        public long RoleId { get; set; }
    }
}
