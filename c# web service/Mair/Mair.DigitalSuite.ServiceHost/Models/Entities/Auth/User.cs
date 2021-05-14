using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Entities.Auth
{
    [Table("Users", Schema = "auth")]
    public class User : EntityBaseWithDates
    {


        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
