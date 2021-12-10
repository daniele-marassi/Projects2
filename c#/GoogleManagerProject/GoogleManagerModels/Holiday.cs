using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class Holiday
{
        public string Description { get; set; }
        public string Summary { get; set; }
        public bool IsHoliday { get { return true; } }
        public Start Start { get; set; }
    }

    public class Start
    {
        public DateTime? Date { get; set; }
    }
}
