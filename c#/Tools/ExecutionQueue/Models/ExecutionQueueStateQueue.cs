
namespace Tools.ExecutionQueue.Models
{
    public enum ExecutionQueueStateQueue
    {
        Executed = 0,
        Failed = 1,
        AttemptToStart = 2,
        RunningStep1 = 3,
        RunningStep2 = 4,
        NONE = 5
    }
}
