using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAttestati
{
    /// <summary>
    /// Configurations
    /// </summary>
    public class Configurations
    {
        public string SepPath { get; set; }
        public string ConnectionStringSepDb { get; set; }
        public bool LocalDb { get; set; }
        public string SepDataPath { get; set; }
        public int StartRowPosition { get; set; }
        public int QuantityRows { get; set; }
    }
}
