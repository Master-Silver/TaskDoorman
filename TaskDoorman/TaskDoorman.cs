using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDoorman
{
    public class TaskDoorman : ITaskDoorman
    {
        private readonly SemaphoreSlim semaphoreForSetUpDoorman = new SemaphoreSlim(1, 1);
        private readonly ConcurrentBag<Task> waitingTasksQueue = new ConcurrentBag<Task>();
        private int waitingTasksCount = 0;
        private volatile bool doWait = false;

        public int WaitingTasksCount { get => waitingTasksCount; }

        public async Task<bool> SetUpDoorman()
        {
            await semaphoreForSetUpDoorman.WaitAsync();

            try
            {
                if (doWait == false)
                {
                    doWait = true;

                    return true;
                }

                return false;
            }
            finally
            {
                semaphoreForSetUpDoorman.Release();
            }
        }

        public Task WaitAsyncIfNecessary()
        {
            Interlocked.Increment(ref waitingTasksCount);

            Task task = new Task(() => { Interlocked.Decrement(ref waitingTasksCount); });

            if (doWait)
            {
                waitingTasksQueue.Add(task);

                if (doWait == false)
                {
                    StartTaskOnlyIfNotStartedYet(task);
                }
            }
            else
            {
                StartTaskOnlyIfNotStartedYet(task);
            }

            return task;
        }

        public void ReleaseWaitingTasks()
        {
            doWait = false;

            while (false == waitingTasksQueue.IsEmpty)
            {
                if (waitingTasksQueue.TryTake(out Task task))
                {
                    StartTaskOnlyIfNotStartedYet(task);
                }
            }
        }

        private void StartTaskOnlyIfNotStartedYet(Task task)
        {
            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }
        }
    }
}
