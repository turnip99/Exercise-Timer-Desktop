using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Exercise_Timer
{
    public class Exercise_Selecter
    {
        List<Exercise> ExerciseList = new List<Exercise>();
        List<SpecificExercise> tempList = new List<SpecificExercise>();
        int time;
        Random rnd = new Random();

        public Exercise_Selecter(int time, bool floor, bool wall, bool band)
        {
            this.time = time;
            string line;
            string name = "";
            int minTime = 0;
            int maxTime = 0;
            int downtime = 0;
            int minReps = 0;
            int maxReps = 0;
            char eachSide = 'a';
            char resource = 'a';
            StreamReader file = new StreamReader(@"./Exercises.txt");
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                if (line[line.Length - 2] == 'N' || (line[line.Length - 2] == 'F' && floor == true) || (line[line.Length - 2] == 'W' && wall == true) || (line[line.Length - 2] == 'B' && band == true))
                {
                    line.ToCharArray();
                    StringBuilder builder = new StringBuilder();
                    int counter = 0;
                    for (int j = 0; j < line.Length; j++)
                    {
                        if (line[j] == '/')
                        {
                            switch (counter)
                            {
                                case 0: name = builder.ToString(); break;
                                case 1: minTime = int.Parse(builder.ToString()); break;
                                case 2: maxTime = int.Parse(builder.ToString()); break;
                                case 3: downtime = int.Parse(builder.ToString()); break;
                                case 4: minReps = int.Parse(builder.ToString()); break;
                                case 5: maxReps = int.Parse(builder.ToString()); break;
                                case 6: eachSide = char.Parse(builder.ToString()); break;
                                case 7: resource = char.Parse(builder.ToString()); break;
                                default: break;
                            }
                            builder.Clear();
                            counter++;
                        }
                        else
                        {
                            builder.Append(line[j].ToString());
                        }
                    }
                    ExerciseList.Add(new Exercise(name, minTime, maxTime, downtime, minReps, maxReps, eachSide, resource));
                }
            }
            file.Close();
        }

        public string GetExercises()
        {
            int currentTime;
            bool fail;
            do
            {
                fail = false;
                tempList.Clear();
                foreach (Exercise item in ExerciseList)
                {
                    item.used = false;
                }
                int timeTemp = 0;
                int repsTemp = 0;
                int totalTimeTemp = 0;
                currentTime = 0;
                int fails = 0;
                Exercise exercise = GetRandomExercise(ref timeTemp, ref repsTemp, ref totalTimeTemp);
                while (totalTimeTemp > time)
                {
                    exercise = GetRandomExercise(ref timeTemp, ref repsTemp, ref totalTimeTemp);
                    fails++;
                    if (fails > 30)
                    {
                        bool nothing = true;
                        Shuffle(ref ExerciseList);
                        foreach (Exercise item in ExerciseList)
                        {
                            if (item.used == false && ((item.eachSide == 'Y' && (item.minTime + item.downtime) * item.minReps * 2 < time) || (item.eachSide == 'N' && (item.minTime + item.downtime) * item.minReps < time)))
                            {
                                exercise = item;
                                timeTemp = exercise.minTime;
                                repsTemp = exercise.minReps;
                                if (exercise.eachSide == 'Y')
                                {
                                    totalTimeTemp = (timeTemp + item.downtime) * repsTemp * 2;
                                }
                                else
                                {
                                    totalTimeTemp = (timeTemp + item.downtime) * repsTemp;
                                }
                                while (totalTimeTemp < time)
                                {
                                    repsTemp += 1;
                                    if (exercise.eachSide == 'Y')
                                    {
                                        totalTimeTemp = (timeTemp + item.downtime) * repsTemp * 2;
                                    }
                                    else
                                    {
                                        totalTimeTemp = (timeTemp + item.downtime) * repsTemp;
                                    }
                                }
                                repsTemp -= 1;
                                if (exercise.eachSide == 'Y')
                                {
                                    totalTimeTemp = (timeTemp + item.downtime) * repsTemp * 2;
                                }
                                else
                                {
                                    totalTimeTemp = (timeTemp + item.downtime) * repsTemp;
                                }
                                nothing = false;
                                break;
                            }
                        }
                        if (nothing)
                        {
                            return "No exercises could be found\nsuitable for this time.";
                        }
                    }
                }
                currentTime += (totalTimeTemp + 5);
                tempList.Add(new SpecificExercise(exercise.name, exercise.minTime, exercise.maxTime, exercise.downtime, exercise.minReps, exercise.maxReps, exercise.eachSide, repsTemp, timeTemp, totalTimeTemp));
                MarkAsComplete(exercise.name);
                fails = 0;
                exercise = GetRandomExercise(ref timeTemp, ref repsTemp, ref totalTimeTemp);
                while (!GetIfTimeIsClose(currentTime, time) && fails < 30)
                {
                    StringBuilder x = new StringBuilder();
                    foreach (Exercise item in ExerciseList)
                    {
                        x.AppendLine(item.name + " = " + item.used);
                    }
                    exercise = GetRandomExercise(ref timeTemp, ref repsTemp, ref totalTimeTemp);
                    while (currentTime + totalTimeTemp > time && fails < 30)
                    {
                        exercise = GetRandomExercise(ref timeTemp, ref repsTemp, ref totalTimeTemp);
                        fails++;
                    }
                    if (fails < 30)
                    {
                        fails = 0;
                        tempList.Add(new SpecificExercise(exercise.name, exercise.minTime, exercise.maxTime, exercise.downtime, exercise.minReps, exercise.maxReps, exercise.eachSide, repsTemp, timeTemp, totalTimeTemp));
                        MarkAsComplete(exercise.name);
                        currentTime += (totalTimeTemp + 5);
                        if (AllUsed())
                        {
                            foreach (Exercise item in ExerciseList)
                            {
                                item.used = false;
                            }
                        }
                    }
                }
                System.Windows.MessageBox.Show("starta" + currentTime);
                currentTime = RecalculateCurrentTime(tempList);
                System.Windows.MessageBox.Show("startb" + currentTime);
                if (!GetIfTimeIsClose(currentTime, time))
                {
                    foreach (SpecificExercise item in tempList)
                    {
                        while (item.reps < item.maxReps && !GetIfTimeIsClose(currentTime + (item.time + item.downtime), time))
                        {
                            item.reps++;
                        }
                        while (item.time < item.maxTime && !GetIfTimeIsClose(currentTime + (repsTemp), time))
                        {
                            item.time++;
                        }
                        if (item.eachSide == 'Y')
                        {
                            item.totalTime = (item.time + item.downtime) * item.reps * 2;
                        }
                        else
                        {
                            item.totalTime = (item.time + item.downtime) * item.reps;
                        }
                    }
                    System.Windows.MessageBox.Show("a" + currentTime);
                }
                if (!GetIfTimeIsClose(currentTime, time))
                {
                    fails = 0;
                    exercise = GetRandomExercise(ref timeTemp, ref repsTemp, ref totalTimeTemp);
                    while ((!GetIfTimeIsClose(currentTime + totalTimeTemp + 5, time) || ((currentTime + totalTimeTemp + 5) > time + (2 + (time / 60)))) && fails < 50)
                    {
                        exercise = GetRandomExercise(ref timeTemp, ref repsTemp, ref totalTimeTemp);
                        fails++;
                    }
                    if (fails < 50)
                    {
                        tempList.Add(new SpecificExercise(exercise.name, exercise.minTime, exercise.maxTime, exercise.downtime, exercise.minReps, exercise.maxReps, exercise.eachSide, repsTemp, timeTemp, totalTimeTemp));
                        currentTime += (totalTimeTemp + 5);
                    }
                    System.Windows.MessageBox.Show("b" + currentTime);
                }
                if (!GetIfTimeIsClose(currentTime, time))
                {
                    int smallposition = 0;
                    int smallsize = tempList[0].time;
                    for (int i = 1; i < tempList.Count; i++)
                    {
                        if (tempList[i].time < smallsize && tempList[i].reps < tempList[i].maxReps)
                        {
                            smallposition = i;
                            smallsize = tempList[i].time;
                        }
                    }
                    int howManyExtra = 0;
                    while (currentTime + ((tempList[smallposition].time + tempList[smallposition].downtime) * howManyExtra) < time + (2 + (time / 60)) && tempList[smallposition].reps + howManyExtra < tempList[smallposition].maxReps)
                    {
                        howManyExtra++;
                    }
                    if (howManyExtra > 0)
                    {
                        tempList[smallposition].reps += howManyExtra;
                        tempList[smallposition].totalTime += (tempList[smallposition].time + tempList[smallposition].downtime) * howManyExtra;
                        currentTime = RecalculateCurrentTime(tempList);
                    }
                    System.Windows.MessageBox.Show("c" + currentTime);
                }
                if (!GetIfTimeIsClose(currentTime, time))
                {
                    int smallposition = 0;
                    int smallsize = tempList[0].reps;
                    for (int i = 1; i < tempList.Count; i++)
                    {
                        if (tempList[i].reps < smallsize && tempList[i].time < tempList[i].maxTime)
                        {
                            smallposition = i;
                            smallsize = tempList[i].reps;
                        }
                    }
                    int howManyExtra = 0;
                    while (currentTime + ((tempList[smallposition].reps) * howManyExtra) < time + (2 + (time / 60)) && tempList[smallposition].time + howManyExtra < tempList[smallposition].maxTime)
                    {
                        howManyExtra++;
                    }
                    if (howManyExtra > 0)
                    {
                        tempList[smallposition].time += howManyExtra;
                        tempList[smallposition].totalTime += (tempList[smallposition].reps * howManyExtra);
                        currentTime = RecalculateCurrentTime(tempList);
                    }
                    System.Windows.MessageBox.Show("d" + currentTime);
                }
                bool changed = true;
                while (currentTime > time + (3 + (time / 60)) && changed)
                {
                    changed = false;
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (tempList[i].time > tempList[i].minTime)
                        {
                            changed = true;
                            tempList[i].time -= 1;
                            tempList[i].totalTime -= tempList[i].reps;
                            currentTime -= tempList[i].reps;
                        }
                        if (currentTime > time + (2 + (time / 60)))
                        {
                            break;
                        }
                        if (tempList[i].reps > tempList[i].minReps)
                        {
                            changed = true;
                            tempList[i].reps -= 1;
                            tempList[i].totalTime -= (tempList[i].time + tempList[i].downtime);
                            currentTime -= tempList[i].reps;
                        }
                        if (currentTime > time + (2 + (time / 60)))
                        {
                            break;
                        }
                    }
                    System.Windows.MessageBox.Show("e" + currentTime);
                }

                currentTime = RecalculateCurrentTime(tempList);
                if (!GetIfTimeIsClose(currentTime, time) || currentTime > time + (2 + (time / 60)))
                {
                    fail = true;
                }

            } while (fail == true);
            currentTime = RecalculateCurrentTime(tempList);
            System.Windows.MessageBox.Show("end" + currentTime);
            StringBuilder builder = new StringBuilder();
            foreach (SpecificExercise item in tempList)
            {
                builder.Append("\n" + item.name + " (" + item.time + "s) x" + item.reps);
                if (item.eachSide == 'Y')
                {
                    builder.Append(" (each side)");
                }
            }
            //builder.AppendLine("\nTotal time: " + currentTime + "s.");
            return builder.ToString();
        }

        private Exercise GetRandomExercise(ref int timeTemp, ref int repsTemp, ref int totalTimeTemp)
        {
            Exercise exercise = ExerciseList[rnd.Next(0, ExerciseList.Count)];
            while (exercise.used)
            {
                exercise = ExerciseList[rnd.Next(0, ExerciseList.Count)];
            }
            timeTemp = rnd.Next(exercise.minTime, exercise.maxTime + 1);
            repsTemp = rnd.Next(exercise.minReps, exercise.maxReps + 1);
            totalTimeTemp = (timeTemp + exercise.downtime) * repsTemp;
            if (exercise.eachSide == 'Y')
            {
                totalTimeTemp *= 2;
            }
            return exercise;
        }

        private bool GetIfTimeIsClose(int currentTime, int totalTime) //Enters this method extra time when broken.
        {
            int leeway = 5 + (totalTime / 60);
            if (currentTime > totalTime - leeway)
            {
                return true;
            }
            return false;
        }

        private void Shuffle(ref List<Exercise> list)
        {
            int n = list.Count;
            Exercise value;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void MarkAsComplete(string name)
        {
            for (int i = 0; i < ExerciseList.Count; i++)
            {
                if (ExerciseList[i].name == name)
                {
                    ExerciseList[i].used = true;
                }
            }
        }

        private bool AllUsed()
        {
            for (int i = 0; i < ExerciseList.Count; i++)
            {
                if (ExerciseList[i].used == false)
                {
                    return false;
                }
            }
            return true;
        }

        private int RecalculateCurrentTime(List<SpecificExercise> list)
        {
            int currenttime = 0;
            foreach (SpecificExercise item in list)
            {
                currenttime += (item.totalTime + 5);
            }
            return currenttime;
        }
    }
}
