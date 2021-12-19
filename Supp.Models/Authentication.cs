using Supp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    [Table("Authentications", Schema = "auth")]
    public class Authentication
    {
        public long Id { get; set; }

        public string Password { get; set; }

        public bool PasswordExpiration { get; set; }

        public int PasswordExpirationDays { get; set; }

        public bool Enable { get; set; }

        public DateTime CreatedAt { get; set; }
        public long UserId { get; set; }

        public DateTime InsDateTime { get;}
    }
}
