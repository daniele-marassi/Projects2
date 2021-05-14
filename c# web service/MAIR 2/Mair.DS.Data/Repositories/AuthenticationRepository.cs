using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.Auth;

namespace Mair.DS.Data.Repositories
{
    public class AuthenticationRepository : EFRepository<Authentication, AutomationContext>
    {
        public AuthenticationRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
