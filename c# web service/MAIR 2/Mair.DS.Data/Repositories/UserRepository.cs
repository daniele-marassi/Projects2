using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.Auth;

namespace Mair.DS.Data.Repositories
{
    public class UserRepository : EFRepository<User, AutomationContext>
    {
        public UserRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
