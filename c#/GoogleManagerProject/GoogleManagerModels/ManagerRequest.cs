using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class ManagerRequest
    {
        public long GoogleAccountId { get; set; }
        public string Account { get; set; }
        public string FolderToFilter { get; set; }
        public string Action { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public int MinThumbnailSize { get; set; }
        public int MaxThumbnailSize { get; set; }
        public Auth Auth { get; set; }
        public string TokenFileInJson { get; set; }
        public string GooglePublicKey { get; set; }
        public string RefreshToken { get; set; }
    }
}
