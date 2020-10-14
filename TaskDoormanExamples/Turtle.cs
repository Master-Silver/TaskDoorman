using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskDoormanExamples
{
    public class Turtle
    {
        public static readonly string Appearance = "<O>";
        public static readonly Random random = new Random();

        public int SpeedPerSecond { get; set; } = 1;
        public int TraveledDistance { get; private set; }
        public Race Race { get; private set; }

        public Turtle(Race race)
        {
            this.Race = race;
        }

        public async Task RunToFinish()
        {
            while (false == DidFinishTrack())
            {
                SpeedPerSecond = random.Next(1, 5);
                await Task.Delay(1000 / SpeedPerSecond);
                await MakeStep();
            }
        }

        private async Task MakeStep()
        {
            TraveledDistance++;
            await Race.UpdateDisplay();
        }

        private bool DidFinishTrack()
        {
            if (TraveledDistance >= Race.Racetrack.TrackLength)
            {
                return true;
            }

            return false;
        }
    }
}
