using System;
using System.Collections.Generic;

namespace Mair.DigitalSuite.Dashboard.Models.Dto.Auth
{
    public class TokenDto
    {
        public string Token { get; set; }

        public int ExpiresInSeconds { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public List<string> Roles { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }

        public string Error_description { get; set; }
    }
}