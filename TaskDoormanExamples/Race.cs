using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskDoormanExamples
{
    public class Race
    {
        public Turtle[] Participants { get; private set; }
        public TurtleRacetrack Racetrack { get; private set; }

        public Race (int numberOfParticipants, int trackLength)
        {
            Racetrack = new TurtleRacetrack(trackLength);
            Participants = new Turtle[numberOfParticipants];

            for (int i = 0; i < numberOfParticipants; i++)
            {
                Participants[i] = new Turtle(this);
            }
        }

        public async Task PerformRace()
        {
            IList<Task> tasks = new List<Task>();

            for (int i = 0; i < Participants.Length; i++)
            {
                tasks.Add(Participants[i].RunToFinish());
            }

            await Task.WhenAll(tasks);
        }

        public async Task UpdateDisplay()
        {
            await Racetrack.DrawRaceTrack(Participants);
        }
    }
}
