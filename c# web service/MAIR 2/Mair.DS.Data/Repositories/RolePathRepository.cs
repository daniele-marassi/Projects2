using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.Auth;

namespace Mair.DS.Data.Repositories
{
    public class RolePathRepository : EFRepository<RolePath, AutomationContext>
    {
        public RolePathRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
