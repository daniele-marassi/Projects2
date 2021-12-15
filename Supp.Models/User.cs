using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    [Table("Users", Schema = "auth")]
    public class User
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string CustomizeParams { get; set; }

        public System.DateTime InsDateTime { get; set;}
    }
}
