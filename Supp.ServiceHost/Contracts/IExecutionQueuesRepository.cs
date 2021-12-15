using Supp.Models;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IExecutionQueuesRepository
    {
        Task<ExecutionQueueResult> GetAllExecutionQueues();

        Task<ExecutionQueueResult> GetExecutionQueuesById(long id);

        Task<ExecutionQueueResult> UpdateExecutionQueue(ExecutionQueueDto dto);

        Task<ExecutionQueueResult> AddExecutionQueue(ExecutionQueueDto dto);

        Task<ExecutionQueueResult> DeleteExecutionQueueById(long id);
    }
}
