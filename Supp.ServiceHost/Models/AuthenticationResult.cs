using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Models
{
    public class AuthenticationResult
    {

        public List<AuthenticationDto> Data { get; set; }

        public ResultType ResultState { get; set; }

        public bool Successful { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Message { get; set; }

        public Exception OriginalException { get; set; }
    }
}
