using Mair.DigitalSuite.Dashboard.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Automation
{
    public class TagDto : BusinessBaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public long NodeId { get; set; }
        public string Address { get; set; }
        public bool Enable { get; set; }

        public string TagName { get; set; }
        public IEnumerable<NodeDto> Nodes { get; set; }
    }
}
