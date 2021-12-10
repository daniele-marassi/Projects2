using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class EditCalendarEventRequest
    {
        public Auth Auth { get; set; }
        public TokenFile TokenFile { get; set; }
        public string Account { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public DateTime? EventDateStart { get; set; }
        public DateTime? EventDateEnd { get; set; }
        public List<int?> NotificationMinutes { get; set; }
        public GoogleCalendarColors? Color { get; set; }
        public DateTime? TimeMin { get; set; }
        public DateTime? TimeMax { get; set; }
        public string SummaryToSearch { get; set; }
        public string IdToSearch { get; set; }
    }
}
