using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Models
{
    public class GoogleDriveAccountDto
    {
        public long Id { get; set; }
        public string Account { get; set; }
        public string FolderToFilter { get; set; }
        public long GoogleDriveAuthId { get; set; }
        public long UserId { get; set; }
        public System.DateTime InsDateTime { get; set; }

        public IEnumerable<GoogleDriveAuthDto> GoogleDriveAuths { get; set; }

        public IEnumerable<UserDto> Users { get; set; }
    }
}
