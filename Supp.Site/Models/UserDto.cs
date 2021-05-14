using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Models
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CustomizeParams { get; set; }

        [Display(Name = "User FullName")]
        public string UserFullName { get; set; }

        public string ConfigDefaultInJson { get; set; }

        public System.DateTime InsDateTime { get; set;}
    }
}
