using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Dto.Auth
{
    public class UserRoleTypeDto: EntityBaseWithDates
    {

        public string Type { get; set; }

    }
}
