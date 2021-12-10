using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    public class MediaResult
    {
        public List<MediaDto> Data { get; set; }
        public ResultType ResultState { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
        public Exception OriginalException { get; set; }
    }
}
