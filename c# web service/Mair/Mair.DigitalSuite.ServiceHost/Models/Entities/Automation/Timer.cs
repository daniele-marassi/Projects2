using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Entities.Automation
{
    [Table("Timers", Schema = "tag")]
    public class Timer : BusinessBaseEntity
    {

        public string Inteval { get; set; }
    }
}
