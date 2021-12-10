using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    [Serializable]
    public class ClaimsDto
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Configuration Configuration { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
