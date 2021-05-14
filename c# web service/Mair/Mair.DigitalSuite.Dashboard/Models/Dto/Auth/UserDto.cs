using Mair.DigitalSuite.Dashboard.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Auth
{
    public class UserDto : EntityBaseWithDates
    {

        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Display(Name = "User FullName")]
        public string UserFullName { get; set; }
    }
}
