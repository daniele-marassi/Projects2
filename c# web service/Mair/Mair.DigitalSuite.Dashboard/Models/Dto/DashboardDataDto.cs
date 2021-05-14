using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Dto
{
    public class DashBoardDataDto
    {
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public byte EventType { get; set; }
        public string TimerName { get; set; }
        public string TimerInterval { get; set; }
        public string NodeName { get; set; }
        public string NodeDescription { get; set; }
        public string PlcConnectionString { get; set; }
        public string PlcDriver { get; set; }
        public string TagName { get; set; }
        public string TagDescription { get; set; }
        public string TagValue { get; set; }
    }
}
