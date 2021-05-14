using Mair.DS.Data.Context;
using Mair.DS.Data.EntityFramework;
using Mair.DS.Models.Entities.EventManager;
using Mair.DS.Models.Entities.EventManager.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DS.Data.Repositories.EventManager
{
    public class EventRepository : EFRepository<Event, EventManagerContext>
    {
        public EventRepository(EventManagerContext context) 
            : base(context)
        {
        }

        public override Task<List<Event>> Read()
        {
            var events = base.Read().Result;

            //foreach (var eve in events)
            //{
            //    switch (eve.Action.ActionTypeId)
            //    {
            //        case ActionType.Database:
            //            DbAction dbAction = Context.DbActions.Find(eve.Action.ActionId);
            //            dbAction.DbActionDetails = Context.DbActionDetails.ToList().FindAll(d => d.DbActionsId == dbAction.Id);
            //            eve.Action = dbAction;
            //            break;
            //        case ActionType.Email:
            //            eve.Action = Context.EmailActions.Find(eve.Action.ActionId);
            //            break;
            //        default:
            //            eve.Action = Context.DbActions.Find(eve.Action.ActionId);
            //            break;
            //    }
            //}

            return Task.FromResult(events);
        }
    }
}
