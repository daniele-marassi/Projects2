using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuppModels
{
    [Table("ExecutionQueues", Schema = "dbo")]
    public class ExecutionQueue
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string FullPath { get; set; }
        public string Arguments { get; set; }
        public string Output { get; set; }
        public string Host { get; set; }
        public string StateQueue { get; set; }


        [Column(TypeName = "datetime")]
        public System.DateTime InsDateTime { get; set;}
    }
}
