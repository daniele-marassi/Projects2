using Mair.DS.Client.Web.Models.Dto.Auth;
using Mair.DS.Client.Web.Models.Dto.Automation;
using System;
using System.Collections.Generic;

namespace Mair.DS.Client.Web.Models.Results.Automation
{
    public class TagResult
    { 
        public List<TagDto> Data { get; set; }

        public ResultType ResultState { get; set; }

        public bool Successful { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Message { get; set; }

        public Exception OriginalException { get; set; }
    }
}
