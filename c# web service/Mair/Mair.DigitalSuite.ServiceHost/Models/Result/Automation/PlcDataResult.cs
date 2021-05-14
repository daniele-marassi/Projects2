using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Result.Automation
{
    public class PlcDataResult
    {
        public List<PlcDataDto> Data { get; set; }

        public ResultType ResultState { get; set; }

        public bool Successful { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Message { get; set; }

        public Exception OriginalException { get; set; }
    }
}
