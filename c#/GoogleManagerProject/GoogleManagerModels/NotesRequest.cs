using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class NotesRequest
    {
        public Auth Auth { get; set; }
        public TokenFile TokenFile { get; set; }
        public string Account { get; set; }
        public DateTime? TimeMin { get; set; }
        public DateTime? TimeMax { get; set; }
    }
}
