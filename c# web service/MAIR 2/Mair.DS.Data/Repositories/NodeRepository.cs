using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Data.Repositories
{
    public class NodeRepository : EFRepository<Node, AutomationContext>
    {
        public NodeRepository(AutomationContext context)
            : base(context)
        {

        }
    }
}
