using Mair.DigitalSuite.Dashboard.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Auth
{
    public class UserRoleTypeDto : EntityBaseWithDates
    {

        public string Type { get; set; }

        public bool Selected { get; set; }
        public string TypeName { get; set; }
    }
}
