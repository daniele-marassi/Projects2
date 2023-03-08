using Tools.ExecutionQueue.Models;
using System.Threading.Tasks;
using System;

namespace Tools.ExecutionQueue.Contracts
{
    public interface IExecutionQueuesRepository
    {
        Task<ExecutionQueueResult> GetAllExecutionQueues();

        Task<ExecutionQueueResult> GetExecutionQueuesById(long id);

        Task<ExecutionQueueResult> UpdateExecutionQueue(ExecutionQueueDto dto);

        Task<ExecutionQueueResult> AddExecutionQueue(ExecutionQueueDto dto);

        Task<ExecutionQueueResult> DeleteExecutionQueueById(long id);

        Task<ExecutionQueueResult> GetQueues(string host, string stateQueue, DateTime scheduledDateTime);
    }
}
