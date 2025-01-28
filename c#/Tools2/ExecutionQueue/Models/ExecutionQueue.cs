using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tools.ExecutionQueue.Models
{
    [Table("ExecutionQueues", Schema = "dbo")]
    public class ExecutionQueue
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string FullPath { get; set; }
        public string Arguments { get; set; }
        public string Output { get; set; }
        public long WebSpeechId { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string Host { get; set; }
        public string StateQueue { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime InsDateTime { get; }
    }
}
