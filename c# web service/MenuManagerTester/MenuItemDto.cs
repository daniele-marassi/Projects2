using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MenuItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? ParentId { get; set; }

        public int Order { get; set; }

        public string Link { get; set; }

        public bool Enable { get; set; }

        public bool Visible { get; set; }
    }
}
