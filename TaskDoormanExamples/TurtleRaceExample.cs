using System;
using System.Threading.Tasks;

namespace TaskDoormanExamples
{
    class TurtleRaceExample
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("How long is the racetrack?");
            int trackLength = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("How many turtles are racing?");
            int numberOfTurtles = Convert.ToInt32(Console.ReadLine());

            Console.Clear();

            Race race = new Race(numberOfTurtles, trackLength);

            await race.PerformRace();
        }
    }
}
