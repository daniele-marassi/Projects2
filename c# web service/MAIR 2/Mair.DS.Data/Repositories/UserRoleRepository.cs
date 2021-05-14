using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.Auth;

namespace Mair.DS.Data.Repositories
{
    public class UserRoleRepository : EFRepository<UserRole, AutomationContext>
    {
        public UserRoleRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
