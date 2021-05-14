using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;
using Mair.DigitalSuite.ServiceHost.Models.Result.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contracts
{
    public interface IEventsRepository
    {
        Task<EventResult> GetAllEvents();

        Task<EventResult> GetEventsById(long id);

        Task<EventResult> UpdateEvent(EventDto dto);

        Task<EventResult> AddEvent(EventDto dto);

        Task<EventResult> DeleteEventById(long id);
    }
}
