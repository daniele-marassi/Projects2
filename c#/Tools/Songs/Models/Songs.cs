using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tools.Songs.Models
{
    [Table("Songs", Schema = "dbo")]
    public class Songs
    {
        public long Id { get; set; }
        public string FullPath { get; set; }
        public string Position { get; set; }
        public int Order { get; set; }
        public bool Listened { get; set; }

        [DataType(DataType.DateTime)]
        public System.DateTime InsDateTime { get; }
    }
}
