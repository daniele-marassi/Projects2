using Mair.DS.Engines.TagDispatcher;
using Mair.DS.Common;
using Mair.DS.Common.JsonDeserializers;
using Mair.DS.Common.Loggers;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories.EventManager;
using Mair.DS.Models.Entities.Automation;
using Mair.DS.Models.Entities.EventManager;
using Mair.DS.Models.Entities.EventManager.Conditions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mair.DS.Engines.Core.EventManager
{
    public class EventManager : IEventManager
    {
        public string Name { get; set; }
        private ILogger logger;
        private ITagDispatcher tagDispatcher;
        private EventManagerContext eventManagerContext;
        private EventRepository eventRepository;
        public List<Event> events;
        private List<TagValue> tagValuesCache;
        private Timer loadCacheTimer;

        public EventManager()
        {
            Name = "EventActionEngine";
            logger = Instances.GetInstance<ILogger>();
            tagDispatcher = Instances.GetInstance<ITagDispatcher>();
        }

        public void Init()
        {
            return;
            logger.LogInfo("Inizio Inizializzazione");

            eventManagerContext = Instances.GetInstance<EventManagerContext>();
            eventRepository = new EventRepository(eventManagerContext);

            events = eventRepository.Read().Result;

            ConfigureConditionDetail();

            //loadCacheTimer = new Timer(LoadCache, true, 0, 5000);

            logger.LogInfo("Fine Inizializzazione");

        }

        private void LoadCache(object state)
        {
            tagValuesCache = tagDispatcher.ReadAllTagsValue();
        }

        private void ConfigureConditionDetail()
        {
            foreach (var eve in events)
            {
                Condition condition = MairJsonSerializer.Deserialize<Condition>(eve.Condition.Json);
                switch (condition.Type)
                {
                    case ConditionType.SimpleTag:
                        eve.Condition = (SimpleTagCondition)condition;
                        break;
                    case ConditionType.Event:
                        eve.Condition = MairJsonSerializer.Deserialize<MairEventCondition>(eve.Condition.Json);
                        break;
                    case ConditionType.MairEvent:
                        eve.Condition = MairJsonSerializer.Deserialize<MairEventCondition>(eve.Condition.Json);
                        break;
                    default:
                        break;
                }
            }
        }

        public void CheckConditions(object sender, object e)
        {
            //TODO: Controlla tutte le condizione e se vere esegue l'azione dell'evento
            logger.LogInfo(string.Format("Mi ha evocato {0} e mi ha detto queste cose {1}", sender, e));

        }

        public List<string> GetConfiguration()
        {
            throw new System.NotImplementedException();
        }
    }
}
