using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Base
{
    public class BusinessBaseEntity : EntityBaseWithDates
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
