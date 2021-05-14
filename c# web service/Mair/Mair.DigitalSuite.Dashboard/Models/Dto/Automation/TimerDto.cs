using Mair.DigitalSuite.Dashboard.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Automation
{
    public class TimerDto : BusinessBaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Inteval { get; set; }
        public string TimerName { get; set; }
    }
}
