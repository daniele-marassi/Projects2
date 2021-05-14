using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Auth
{
    [Table("Authentications", Schema = "auth")]
    public class Authentication: EntityBaseWithDates
    {
         
        public string Password { get; set; }

        public bool PasswordExpiration { get; set; }

        public int PasswordExpirationDays { get; set; }

        public bool Enable { get; set; }

        public long UserId { get; set; }

    }
}
