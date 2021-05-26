using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Supp.Site.Models.WebSpeechTypes;

namespace Supp.Site.Models
{
    public class ShortcutDto
    {
        public long Id { get; set; }
        public string Ico { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
    }
}
