using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Dto.Automation
{
    public class TagDto : BusinessBaseEntity
    {

        public long NodeId { get; set; }
        public string Address { get; set; }
        public bool Enable { get; set; }
    }
}
