using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Dto.Auth
{
    public class TokenDto
    {
        public string Token { get; set; }

        public int ExpiresInSeconds { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public List<string> Roles { get; set; }

        public string Message { get; set; }
    }
}
