using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public class GoogleAccountDto
    {
        public long Id { get; set; }
        public string Account { get; set; }
        public string FolderToFilter { get; set; }
        public long GoogleAuthId { get; set; }
        public long UserId { get; set; }
        public string AccountType { get; set; }
        public DateTime InsDateTime { get; set; }

        public IEnumerable<GoogleAuthDto> GoogleAuths { get; set; }

        public IEnumerable<UserDto> Users { get; set; }

        public IEnumerable<(string Id, string AccountType)> AccountTypes { get; set; }
    }
}
