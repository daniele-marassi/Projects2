using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public class MediaDto
    {
        public long Id { get; set; }
        public long GoogleAccountId { get; set; }
        public string FileId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public long Size { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public long? VideoDurationMillis { get; set; }
        public int? VideoHeight { get; set; }
        public int? VideoWidth { get; set; }
        public string ImageTime { get; set; }
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
        public double? ImageLocationAltitude { get; set; }
        public double? ImageLocationLatitude { get; set; }
        public double? ImageLocationLongitude { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public byte[] File { get; set; }
        public byte[] Thumbnail { get; set; }
        public int ThumbnailWidth { get; set; }
        public int ThumbnailHeight { get; set; }
        public DateTime InsDateTime { get; set; }

        public IEnumerable<GoogleAccountDto> GoogleAccounts { get; set; }
    }
}
