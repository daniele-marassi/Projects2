using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Supp.Site.Models
{
    public class UserRoleDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long UserRoleTypeId { get; set; }

        public System.DateTime InsDateTime { get; set;}

        public IEnumerable<UserDto> Users { get; set; }

        public IEnumerable<UserRoleTypeDto> UserRoleTypes { get; set; }
    }
}
