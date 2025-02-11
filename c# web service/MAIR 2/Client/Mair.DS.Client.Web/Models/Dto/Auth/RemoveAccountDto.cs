﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mair.DS.Client.Web.Models.Dto.Auth
{
    public class RemoveAccountDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}