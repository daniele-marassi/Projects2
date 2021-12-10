using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    [Table("GoogleAccounts", Schema = "dbo")]
    public class GoogleAccount
    {
        public long Id { get; set; }
        public string Account { get; set; }
        public string FolderToFilter { get; set; }
        public long GoogleAuthId { get; set; }
        public long UserId { get; set; }
        public string AccountType { get; set; }
        public System.DateTime InsDateTime { get; set;}
    }
}
