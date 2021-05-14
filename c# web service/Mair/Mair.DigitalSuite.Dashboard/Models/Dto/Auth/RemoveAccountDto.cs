using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Auth
{
    public class RemoveAccountDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}