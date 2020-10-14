using System.Threading.Tasks;

namespace TaskDoorman
{
    public interface ITaskDoorman
    {
        int WaitingTasksCount { get; }

        void ReleaseWaitingTasks();
        Task<bool> SetUpDoorman();
        Task WaitAsyncIfNecessary();
    }
}