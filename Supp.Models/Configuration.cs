using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
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
            public string MeteoParameterToTheSalutation { get; set; }
            public bool DescriptionMeteoToTheSalutationActive { get; set; }
            public bool RemindersActive { get; set; }
            public int TimeToResetInSeconds { get; set; }
            public int TimeToEhiTimeoutInSeconds { get; set; }
        }
    }
}
