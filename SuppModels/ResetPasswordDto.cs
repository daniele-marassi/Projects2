using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuppModels
{
    public class ResetPasswordDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}