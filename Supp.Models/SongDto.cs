using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public class SongDto
    {
        public long Id { get; set; }
        public string FullPath { get; set; }
        public string Position { get; set; }
        public int Order { get; set; }
        public bool Listened { get; set; }
        public long DurationInMilliseconds { get; set; }
        public System.DateTime InsDateTime { get; }

        public bool Successful { get; set; }

        public string Title { get; set; }

        public string Host { get; set; }
        public string HostsArray { get; set; }
        public string HostSelected { get; set; }
        public string Command { get; set; }
        public string PlayListArray { get; set; }
        public string PlayListSelected { get; set; }
        public bool Shuffle { get; set; }
        public bool Repeat { get; set; }
        public int Volume { get; set; }
    }
}
