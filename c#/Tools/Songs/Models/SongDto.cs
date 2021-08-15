namespace Tools.Songs.Models
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
    }
}