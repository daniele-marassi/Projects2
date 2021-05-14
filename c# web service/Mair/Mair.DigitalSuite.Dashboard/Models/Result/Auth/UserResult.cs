using Mair.DigitalSuite.Dashboard.Models.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Models.Result.Auth
{
    public class UserResult
    {
        public List<UserDto> Data { get; set; }
        public ResultType ResultState { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
        public Exception OriginalException { get; set; }
    }
}
