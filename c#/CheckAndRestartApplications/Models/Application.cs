namespace CheckAndRestartApplications.Models
{
    public class Application
    {
        public string Name { get; set; }
        public long MaxMemoryInByte { get; set; }
        public string ApplicationFullPath { get; set; }
    }
}
