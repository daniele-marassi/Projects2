using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAttestati
{
    /// <summary>
    /// Configurations
    /// </summary>
    public class Configurations
    {
        public string FullPathLocalDb { get; set; }
        public string ConnectionStringRemoteDb { get; set; }
        public string PathToSave { get; set; }
        public bool LocalDb { get; set; }
    }
}
