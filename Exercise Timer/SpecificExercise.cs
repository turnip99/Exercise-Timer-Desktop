using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Timer
{
    class SpecificExercise
    {
        public string name;
        public int minTime;
        public int maxTime;
        public int downtime;
        public int minReps;
        public int maxReps;
        public char eachSide;
        public int reps;
        public int time;
        public int totalTime;
        public SpecificExercise(string name, int minTime, int maxTime, int downtime, int minReps, int maxReps, char eachSide, int reps, int time, int totalTime)
        {
            this.name = name;
            this.minTime = minTime;
            this.maxTime = maxTime;
            this.downtime = downtime;
            this.minReps = minReps;
            this.maxReps = maxReps;
            this.eachSide = eachSide;
            this.reps = reps;
            this.time = time;
            this.totalTime = totalTime;
        }
    }
}
