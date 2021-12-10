using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    public class MediaConfigurationDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int MaxThumbnailSize { get; set; }
        public int MinThumbnailSize { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
        public string UserFullName { get; set; }
        public System.DateTime InsDateTime { get; set; }
    }
}
