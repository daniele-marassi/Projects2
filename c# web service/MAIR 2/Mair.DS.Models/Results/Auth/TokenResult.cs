using Mair.DS.Models.Dto.Auth;
using System;
using System.Collections.Generic;

namespace Mair.DS.Models.Results.Auth
{
    public class TokenResult
    { 
        public List<TokenDto> Data { get; set; }

        public ResultType ResultState { get; set; }

        public bool Successful { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Message { get; set; }

        public Exception OriginalException { get; set; }
    }
}
