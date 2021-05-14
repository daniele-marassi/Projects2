using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Client.Web.Models.Dto.Auth
{
    public class AuthenticationDto: EntityBaseWithDates
    {
         
        public string Password { get; set; }

        public bool PasswordExpiration { get; set; }

        public int PasswordExpirationDays { get; set; }

        public bool Enable { get; set; }

        public long UserId { get; set; }

    }
}
