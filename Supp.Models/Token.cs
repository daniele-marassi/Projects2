using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    [Table("Tokens", Schema = "auth")]
    public class Token
    {
        public long Id { get; set; }

        public string TokenCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int ExpiresInSeconds { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public string RolesInJson { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string ConfigInJson { get; set; }

        public DateTime InsDateTime { get; set; }
    }
}