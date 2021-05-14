using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.Auth;

namespace Mair.DS.Data.Repositories
{
    public class RoleRepository : EFRepository<Role, AutomationContext>
    {
        public RoleRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
