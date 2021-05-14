using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Mair.DS.Engines.Base;
using Mair.DS.Models.Entities.Automation;

namespace Mair.DS.Engines
{
    public class TagEngine : BaseEngine<Tag>
    {
        public TagEngine(AutomationContext context)
            : base( new TagRepository(context))
        {

        }
    }
}
