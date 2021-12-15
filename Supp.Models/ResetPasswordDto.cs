using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Supp.Models
{
    public class ResetPasswordDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}