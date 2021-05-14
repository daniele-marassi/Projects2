using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveManagerModels
{
    public class ManagerResult
    {
        public List<RequestResult> Data { get; set; }
        public ResultType ResultState { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
        public Exception OriginalException { get; set; }         
    }
}
