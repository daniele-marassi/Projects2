using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Data.Repositories
{
    public class TagRepository : EFRepository<Tag, AutomationContext>
    {
        public TagRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
