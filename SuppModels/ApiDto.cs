using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    public class ApiDto
    {
        public string Url { get; set; }
        public string Parameters { get; set; }
        public string ActionType { get; set; }
        public string Roles { get; set; }
    }
}
