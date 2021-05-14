using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DS.Client.Web.Models.Results.Page
{
    public class PageManagerResult
    {
        public int CurrentPage { get; set; }
        public int FromRecord { get; set; }
        public int NumberOfrecords { get; set; }
        public int MaxRecords { get; set; }
        public bool ChangePage { get; set; }
    }
}