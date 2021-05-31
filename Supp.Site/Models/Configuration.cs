using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Models
{
    public class Configuration
    {
        public _General General { get; set; }
        public _Speech Speech { get; set; }

        public class _General
        {
            public string PageSize { get; set; }
            public string Culture { get; set; }
        }

        public class _Speech
        {
            public string HostsArray { get; set; }
            public string HostDefault { get; set; }
            public string ListeningWord1 { get; set; }
            public string ListeningWord2 { get; set; }
            public string ListeningAnswer { get; set; }
            public string Salutation { get; set; }
            public string SpeechWordsCoefficient { get; set; }
            public string MeteoParameterToTheSalutation { get; set; }
        }
    }
}
