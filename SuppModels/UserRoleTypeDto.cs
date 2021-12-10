using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    public class UserRoleTypeDto
    {
        public long Id { get; set; }
        public string Type { get; set; }

        public System.DateTime InsDateTime { get; set;}
        public bool Selected { get; set; }
        public string TypeName { get; set; }
    }
}
