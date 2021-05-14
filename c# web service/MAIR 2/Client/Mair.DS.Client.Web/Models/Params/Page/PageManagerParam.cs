using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DS.Client.Web.Models.Params.Page
{
    public class PageManagerParam
    {
        public IJSRuntime JSRuntime { get; set; }
        public int CurrentPage { get; set; }
        public int FromRecord { get; set; }
        public int NumberOfrecords { get; set; }
        public int MaxRecords { get; set; }
    }
}