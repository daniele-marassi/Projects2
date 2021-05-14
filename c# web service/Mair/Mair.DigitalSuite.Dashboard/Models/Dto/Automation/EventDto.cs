using Mair.DigitalSuite.Dashboard.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Automation
{
    public class EventDto : BusinessBaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public byte Type { get; set; }
        public long TimerId { get; set; }
        public long PlcStartId { get; set; }
        public long PlcEndId { get; set; }
        public long PlcAckId { get; set; }
        public IEnumerable<TimerDto> Timers { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
