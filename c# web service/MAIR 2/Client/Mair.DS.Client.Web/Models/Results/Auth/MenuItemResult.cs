using Mair.DS.Client.Web.Models.Dto;
using System;
using System.Collections.Generic;

namespace Mair.DS.Client.Web.Models.Results.Auth
{
    public class MenuItemResult
    { 
        public List<MenuItemDto> Data { get; set; }

        public ResultType ResultState { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public Exception OriginalException { get; set; }
    }
}
