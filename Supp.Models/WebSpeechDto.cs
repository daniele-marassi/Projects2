using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Supp.Models.WebSpeechTypes;

namespace Supp.Models
{
    public class WebSpeechDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phrase { get; set; }
        public string Operation { get; set; }
        public bool OperationEnable { get; set; }
        public string Parameters { get; set; }
        public string Host { get; set; }
        public string Answer { get; set; }
        public bool FinalStep { get; set; }
        public long UserId { get; set; }
        public string ParentIds { get; set; }
        public string Ico { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public int Step { get; set; }
        public string StepType { get; set; }
        public DateTime InsDateTime { get; set; }

        public string HostsArray { get; set; }
        public string HostSelected { get; set; }
        public string ListeningWord1 { get; set; }
        public string ListeningWord2 { get; set; }
        public string ListeningAnswer { get; set; }
        public string StartAnswer { get; set; }
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
        public bool ResetAfterLoad { get; set; }
        public int TimesToReset { get; set; }
        public bool OnlyRefresh { get; set; }
        public IEnumerable<WebSpeechType> WebSpeechTypes { get { return Supp.Models.WebSpeechTypesUtility.Get(); } }
        public IEnumerable<ShortcutImage> ShortcutImages { get; set; }
        public string NewWebSpeechRequestName { get; set; }
        public bool RecognitionDisable { get; set; }
        public IEnumerable<Host> Hosts { get; set; }
        public IEnumerable<StepType> StepTypes { get { return Supp.Models.StepTypesUtility.Get(); } }
    }
}
