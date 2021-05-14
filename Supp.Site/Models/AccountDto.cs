using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Models
{
    public class AccountDto
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool PasswordExpiration { get; set; }
        [Required]
        public int PasswordExpirationDays { get; set; }
        [Required]
        public bool Enable { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public long[] UserRoleTypeIds { get; set; }

        public IEnumerable<UserRoleTypeDto> UserRoleTypes { get; set; }
    }
}
