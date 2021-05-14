using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Models.Dto.Automation
{
    public class TagSubscriptionDto
    {
        public Tag Tag { get; set; }
        public int Interval { get; set; }
    }
}
