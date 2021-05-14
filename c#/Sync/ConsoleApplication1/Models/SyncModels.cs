using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncScript.Sync.Models
{
    public class SyncModels
    {

        public class SyncFolder
        {
            public List<SyncPath> Folders { get; set; }
            public string Owner { get; set; }
        }

        public class SyncPath
        {
            public string SourcePath { get; set; }
            public string DestinationPath { get; set; }
        }

        public class ListOfFilesRelated
        {
            public string PathMainFolder { get; set; }
            public List<string> PathsFile { get; set; }
        }
    }
}
