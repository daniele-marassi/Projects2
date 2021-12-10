using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    [Table("UserRoles", Schema = "auth")]
    public class UserRole
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long UserRoleTypeId { get; set; }


        public System.DateTime InsDateTime { get; set;}
    }
}
