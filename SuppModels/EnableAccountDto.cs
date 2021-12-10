using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuppModels
{
    public class EnableAccountDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}