using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Mair.DS.Engines.Base;
using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Engines
{
    public class NodeEngine : BaseEngine<Node>
    {
        public NodeEngine(AutomationContext context)
            : base( new NodeRepository(context))
        {

        }
    }
}
