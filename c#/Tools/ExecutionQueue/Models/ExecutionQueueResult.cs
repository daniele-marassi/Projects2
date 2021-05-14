using System;
using System.Collections.Generic;

namespace Tools.ExecutionQueue.Models
{
    public class ExecutionQueueResult
    {
        public List<ExecutionQueueDto> Data { get; set; }
        public ResultType ResultState { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
        public Exception OriginalException { get; set; }
    }
}
