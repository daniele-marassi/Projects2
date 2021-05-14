using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAttestati
{
    public class Attestato
    {
        public int Id_Allievo { get; set; }
        public string FileName { get; set; }
        public Byte[] Pdf { get; set; }
    }
}
