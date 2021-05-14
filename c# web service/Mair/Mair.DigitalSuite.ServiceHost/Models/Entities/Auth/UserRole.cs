using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Entities.Auth
{
    [Table("UserRoles", Schema = "auth")]
    public class UserRole : EntityBaseWithDates
    {


        public long UserId { get; set; }

        public long UserRoleTypeId { get; set; }
    }
}
