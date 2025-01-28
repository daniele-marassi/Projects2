using System;

namespace Tools.ExecutionQueue.Models
{
    public class ExecutionQueueDto
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
        public DateTime InsDateTime { get; set; }
    }
}