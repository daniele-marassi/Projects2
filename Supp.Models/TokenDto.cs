using System;
using System.Collections.Generic;

namespace Supp.Models
{
    public class TokenDto
    {
        public long Id { get; set; }

        public string TokenCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int ExpiresInSeconds { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public string RolesInJson { get; set; }

        public List<string> Roles { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }

        public string Error_description { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string ConfigInJson { get; set; }

        public DateTime InsDateTime { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}