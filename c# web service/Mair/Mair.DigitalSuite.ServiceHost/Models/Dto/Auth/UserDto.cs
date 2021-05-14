using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Dto.Auth
{
    public class UserDto : EntityBaseWithDates
    {


        public string UserName { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
