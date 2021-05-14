using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Entities.Auth
{
    [Table("Authentications", Schema = "auth")]
    public class Authentication: EntityBaseWithDates
    {


        public string Password { get; set; }

        public bool PasswordExpiration { get; set; }

        public int PasswordExpirationDays { get; set; }

        public bool Enable { get; set; }

        public long UserId { get; set; }

    }
}
