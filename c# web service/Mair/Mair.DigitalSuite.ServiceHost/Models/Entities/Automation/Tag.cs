using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Entities.Automation
{
    [Table("Tags", Schema = "tag")]
    public class Tag : BusinessBaseEntity
    {


        public long NodeId { get; set; }
        public string Address { get; set; }
        public bool Enable { get; set; }
    }
}
