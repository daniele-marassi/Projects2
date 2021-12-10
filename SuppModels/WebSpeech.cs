using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    [Table("WebSpeeches", Schema = "dbo")]
    public class WebSpeech
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

        public System.DateTime InsDateTime { get; set;}
    }
}
