using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Dto.Automation
{
    public class NodeDto : BusinessBaseEntity
    {

        public string Driver { get; set; }
        public string ConnectionString { get; set; }
    }
}
