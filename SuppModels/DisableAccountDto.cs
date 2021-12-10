using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuppModels
{
    public class DisableAccountDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}