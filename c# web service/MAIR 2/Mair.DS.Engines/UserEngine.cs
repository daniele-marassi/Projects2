using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Mair.DS.Engines.Base;
using Mair.DS.Models.Entities.Auth;
using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Engines
{
    public class UserEngine : BaseEngine<User>
    {
        public UserEngine(AutomationContext context)
            : base( new UserRepository(context))
        {

        }
    }
}
