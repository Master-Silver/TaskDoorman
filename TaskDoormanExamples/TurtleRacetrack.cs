using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskDoorman;

namespace TaskDoormanExamples
{
    public class TurtleRacetrack
    {
        private readonly ITaskDoorman doorman;
        public int TrackLength { get; private set; }

        public TurtleRacetrack(int trackLength)
        {
            doorman = new TaskDoorman.TaskDoorman();
            TrackLength = trackLength;
        }

        public async Task DrawRaceTrack(Turtle[] turtles)
        {
            if (await doorman.SetUpDoorman())
            {
                try
                {
                    DrawFinishLine(turtles.Length);

                    for (int i = 0; i < turtles.Length; i++)
                    {
                        Console.SetCursorPosition(turtles[i].TraveledDistance, i);
                        DrawTurtle();
                    }
                }
                finally
                {
                    doorman.ReleaseWaitingTasks();
                }

            }
            else
            {
                await doorman.WaitAsyncIfNecessary();
            }
        }

        private void DrawTurtle()
        {
            Console.Write(Turtle.Appearance);
        }

        private void DrawFinishLine(int turtlesCount)
        {
            for (int i = 0; i < turtlesCount; i++)
            {
                Console.SetCursorPosition(TrackLength, i);
                Console.Write("|");
            }
        }
    }
}
