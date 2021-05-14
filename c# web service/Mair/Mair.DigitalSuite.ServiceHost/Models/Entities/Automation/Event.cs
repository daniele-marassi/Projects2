using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Entities.Automation
{
    [Table("Events", Schema = "tag")]
    public class Event: BusinessBaseEntity
    {

        public byte Type { get; set; }
        public long TimerId  { get; set; }
        public long PlcStartId { get; set; }
        public long PlcEndId { get; set; }
        public long PlcAckId { get; set; }
    }
}
