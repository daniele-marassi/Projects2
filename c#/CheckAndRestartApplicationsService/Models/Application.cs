using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckAndRestartApplicationsService.Models
{
    public class Application
    {
        public string Name { get; set; }
        public long MaxMemoryInByte { get; set; }
        public string ApplicationFullPath { get; set; }
    }
}
