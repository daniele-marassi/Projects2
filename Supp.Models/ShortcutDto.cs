using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Supp.Models.WebSpeechTypes;

namespace Supp.Models
{
    public class ShortcutDto
    {
        public long Id { get; set; }
        public string Ico { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public bool Groupable { get; set; }
        public string GroupName { get; set; }
        public int GroupOrder { get; set; }
        public bool HotShortcut { get; set; }  
    }
}
