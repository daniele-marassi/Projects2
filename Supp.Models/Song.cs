using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    [Table("Songs", Schema = "dbo")]
    public class Song
    {
        public long Id { get; set; }
        public string FullPath { get; set; }
        public string Position { get; set; }
        public int Order { get; set; }
        public bool Listened { get; set; }
        public long DurationInMilliseconds { get; set; }
        public DateTime InsDateTime { get; set; }
    }
}
