﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Auth
{
    public class LoginDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}