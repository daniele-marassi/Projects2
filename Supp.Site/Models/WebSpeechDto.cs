using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Supp.Site.Models.WebSpeechTypes;

namespace Supp.Site.Models
{
    public class WebSpeechDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phrase { get; set; }
        public string Operation { get; set; }
        public string Parameters { get; set; }
        public string Host { get; set; }
        public string Answer { get; set; }
        public bool FinalStep { get; set; }
        public long UserId { get; set; }
        public string ParentIds { get; set; }
        public string Ico { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public System.DateTime InsDateTime { get; set;}
        public string HostsArray { get; set; }
        public string HostSelected { get; set; }
        public string ListeningWord1 { get; set; }
        public string ListeningWord2 { get; set; }
        public string ListeningAnswer { get; set; }
        public string StartAnswer { get; set; }
        public bool Implementation { get; set; }
        public int Ehi { get; set; }
        public string Culture { get; set; }
        public bool Application { get; set; }
        public bool AlwaysShow { get; set; } 
        public long ExecutionQueueId { get; set; }
        public string Error { get; set; }
        public bool PrivateInstruction { get; set; }
        public string PreviousPhrase { get; set; }
        public IEnumerable<WebSpeechDto> WebSpeeches { get; set; }
        public long[] WebSpeechIds { get; set; }
        public string ShortcutsInJson { get; set; }
        public bool ResetAfterLogin { get; set; }
        public IEnumerable<WebSpeechType> WebSpeechTypes { get { return Supp.Site.Models.WebSpeechTypes.Get(); } }

        public IEnumerable<ShortcutImage> ShortcutImages { get; set; }
    }
}
