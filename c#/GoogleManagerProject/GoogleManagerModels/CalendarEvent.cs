using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class CalendarEvent
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public bool IsHoliday { get; set; }
        public DateTime? EventDateStart { get; set; }
        public List<DateTime> NotificationDates { get; set; }
    }
}
