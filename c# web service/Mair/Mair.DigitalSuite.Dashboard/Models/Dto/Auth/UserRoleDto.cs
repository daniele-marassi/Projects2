using Mair.DigitalSuite.Dashboard.Models.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Mair.DigitalSuite.Dashboard.Models.Dto.Auth
{
    public class UserRoleDto : EntityBaseWithDates
    {


        public long UserId { get; set; }

        public long UserRoleTypeId { get; set; }

        public IEnumerable<UserDto> Users { get; set; }

        public IEnumerable<UserRoleTypeDto> UserRoleTypes { get; set; }
    }
}
