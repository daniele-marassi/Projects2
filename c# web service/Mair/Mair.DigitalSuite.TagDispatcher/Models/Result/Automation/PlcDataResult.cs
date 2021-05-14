using Mair.DigitalSuite.TagDispatcher.Models.Dto.Automation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DigitalSuite.TagDispatcher.Models.Result.Automation
{
    public class PlcDataResult
    {
        public List<PlcDataDto> Data { get; set; }
        public ResultType ResultState { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
        public Exception OriginalException { get; set; }
    }
}
