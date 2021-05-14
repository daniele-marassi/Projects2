using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Param.Automation
{
    public class PlcDataParam
    {
        public string Driver { get; set; }
        public string ConnectionString { get; set; }
        public string TagAddress { get; set; }
        public string TagValue { get; set; }
    }
}
