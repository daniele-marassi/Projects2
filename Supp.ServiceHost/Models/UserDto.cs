using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Models
{
    public class UserDto
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string CustomizeParams { get; set; }

        public System.DateTime InsDateTime { get; set;}
    }
}
