using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public class AuthenticationDto
    {
        public long Id { get; set; }
        public string Password { get; set; }
        public bool PasswordExpiration { get; set; }
        public int PasswordExpirationDays { get; set; }
        public bool Enable { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UserId { get; set; }

        public System.DateTime InsDateTime { get; set;}
        public IEnumerable<UserDto> Users { get; set; }
    }
}