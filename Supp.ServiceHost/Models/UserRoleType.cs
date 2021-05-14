using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Models
{
    [Table("UserRoleTypes", Schema = "auth")]
    public class UserRoleType
    {
        public long Id { get; set; }

        public string Type { get; set; }


        public System.DateTime InsDateTime { get; set;}
    }
}
