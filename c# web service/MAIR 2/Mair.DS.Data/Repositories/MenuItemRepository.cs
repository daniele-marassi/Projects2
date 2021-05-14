using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities;
using Mair.DS.Models.Entities.Auth;

namespace Mair.DS.Data.Repositories
{
    public class MenuItemRepository : EFRepository<MenuItem, AutomationContext>
    {
        public MenuItemRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
