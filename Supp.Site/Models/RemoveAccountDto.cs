using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Supp.Site.Models
{
    public class RemoveAccountDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}