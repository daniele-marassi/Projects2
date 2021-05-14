using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Dto.Auth
{
    public class AuthenticationDto: EntityBaseWithDates
    {

        public string Password { get; set; }
        public bool PasswordExpiration { get; set; }
        public int PasswordExpirationDays { get; set; }
        public bool Enable { get; set; }
        public long UserId { get; set; }
    }
}
