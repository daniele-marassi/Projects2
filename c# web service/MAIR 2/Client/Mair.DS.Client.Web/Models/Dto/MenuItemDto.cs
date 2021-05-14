using Mair.DS.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DS.Client.Web.Models.Dto
{
    public class MenuItemDto: BusinessBaseEntity
    {

        public long? ParentId { get; set; }

        public int Order { get; set; }

        public string Link { get; set; }

        public bool Enable { get; set; }

        public bool Visible { get; set; }
    }
}
