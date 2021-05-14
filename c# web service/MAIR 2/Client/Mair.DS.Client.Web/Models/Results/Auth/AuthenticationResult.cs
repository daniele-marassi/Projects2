using Mair.DS.Client.Web.Models.Dto.Auth;
using System;
using System.Collections.Generic;

namespace Mair.DS.Client.Web.Models.Results.Auth
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
