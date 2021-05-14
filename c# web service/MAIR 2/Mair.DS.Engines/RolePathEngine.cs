using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Mair.DS.Engines.Base;
using Mair.DS.Models.Entities.Auth;
using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Engines
{
    public class RolePathEngine : BaseEngine<RolePath>
    {
        public RolePathEngine(AutomationContext context)
            : base( new RolePathRepository(context))
        {

        }
    }
}
