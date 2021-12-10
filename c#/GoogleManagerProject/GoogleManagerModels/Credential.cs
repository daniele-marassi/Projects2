using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class Credential
    {
        public Auth Auth { get; set; }
        public AccessProperties AccessProperties { get; set; }
        public string AccessFileName { get; set; }
        public string Account { get; set; }
        public string FolderToFilter { get; set; }
        public long UserId { get; set; }
        public string AccountType { get; set; }
        public string TokenFileInJson { get; set; }
        public string GooglePublicKey { get; set; }     
    }
}
