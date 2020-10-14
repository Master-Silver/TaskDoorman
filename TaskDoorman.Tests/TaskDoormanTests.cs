using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TaskDoorman.Tests
{
    public class TaskDoormanTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task Should_CompletedAllTask_When_Released(int numberOfTasks)
        {
            SemaphoreSlim waitForTheRace = new SemaphoreSlim(0, numberOfTasks);
            IList<Task> listofTasks = new List<Task>(numberOfTasks);
            ITaskDoorman doorman = new TaskDoorman();

            for (int i = 0; i < numberOfTasks; i++)
            {
                listofTasks.Add(Task.Run(async () =>
                {
                    await waitForTheRace.WaitAsync();

                    if (await doorman.SetUpDoorman())
                    {
                        doorman.ReleaseWaitingTasks();
                    }
                    else
                    {
                        await doorman.WaitAsyncIfNecessary();
                    }
                }));
            }

            waitForTheRace.Release(numberOfTasks);
            await Task.WhenAll(listofTasks);
            Assert.Equal(0, doorman.WaitingTasksCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task Should_CompletedAllTask_When_ReleasedAfterYield(int numberOfTasks)
        {
            SemaphoreSlim waitForTheRace = new SemaphoreSlim(0, numberOfTasks);
            IList<Task> listofTasks = new List<Task>(numberOfTasks);
            ITaskDoorman doorman = new TaskDoorman();

            for (int i = 0; i < numberOfTasks; i++)
            {
                listofTasks.Add(Task.Run(async () =>
                {
                    await waitForTheRace.WaitAsync();

                    if (await doorman.SetUpDoorman())
                    {
                        await Task.Yield();
                        doorman.ReleaseWaitingTasks();
                    }
                    else
                    {
                        await doorman.WaitAsyncIfNecessary();
                    }
                }));
            }

            await Task.Yield();
            waitForTheRace.Release(numberOfTasks);
            await Task.Yield();
            await Task.WhenAll(listofTasks);
            Assert.Equal(0, doorman.WaitingTasksCount);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 10)]
        [InlineData(5, 10)]
        [InlineData(10, 10)]
        [InlineData(100, 10)]
        public async Task Should_CompletedAllTaskInTime_When_ReleasedAfterDelay(int numberOfTasks, int delayTime)
        {
            SemaphoreSlim waitForTheRace = new SemaphoreSlim(0, numberOfTasks);
            IList<Task> listofTasks = new List<Task>(numberOfTasks);
            ITaskDoorman doorman = new TaskDoorman();

            for (int i = 0; i < numberOfTasks; i++)
            {
                listofTasks.Add(Task.Run(async () =>
                {
                    await waitForTheRace.WaitAsync();

                    if (await doorman.SetUpDoorman())
                    {
                        await Task.Delay(delayTime);
                        doorman.ReleaseWaitingTasks();
                    }
                    else
                    {
                        await doorman.WaitAsyncIfNecessary();
                    }
                }));
            }

            await Task.Delay(delayTime);
            waitForTheRace.Release(numberOfTasks);
            await Task.Delay(delayTime * 3);

            for (int i = 0; i < listofTasks.Count; i++)
            {
                Assert.True(listofTasks[i].IsCompleted);
            }

            Assert.Equal(0, doorman.WaitingTasksCount);
        }
    }
}