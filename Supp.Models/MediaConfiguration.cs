using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    [Table("MediaConfigurations", Schema = "dbo")]
    public class MediaConfiguration
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int MaxThumbnailSize { get; set; }
        public int MinThumbnailSize { get; set; }

        public DateTime InsDateTime { get; }
    }
}
