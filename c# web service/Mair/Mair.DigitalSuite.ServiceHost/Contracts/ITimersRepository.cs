using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;
using Mair.DigitalSuite.ServiceHost.Models.Result.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contracts
{
    public interface ITimersRepository
    {
        Task<TimerResult> GetAllTimers();

        Task<TimerResult> GetTimersById(long id);

        Task<TimerResult> UpdateTimer(TimerDto dto);

        Task<TimerResult> AddTimer(TimerDto dto);

        Task<TimerResult> DeleteTimerById(long id);
    }
}
