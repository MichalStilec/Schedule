using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace ConsoleApp1
{
    /// <summary>
    /// Main program class responsible for generating and reviewing school schedules.
    /// </summary>
    public class Program
    {
        static Random random = new Random();
        static Reviewer reviewer = new Reviewer();
        static string json = new StreamReader("JsonFiles/data.json").ReadToEnd();
        static List<List<Day>> schedules = new List<List<Day>>();
        static List<(List<Day>, int)> bestSchedules = new List<(List<Day>, int)>();
        static int betterSchedules = 0;
        static int oldSchedulePoints = 0;
        static bool endallthreads = false;
        static int time = 0;
        static int schedulesCount = 0;
        static int reviewedCount = 0;
        static object lockObject = new object();

        static void Main(string[] args)
        {
            // Get the desired runtime from the user
            try
            {
                Console.Write("Please choose the time you want the program to run for: ");
                int timeTemp = Convert.ToInt32(Console.ReadLine());
                time = timeTemp;
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect input, starting the program defaultly for 3 minutes");
                time = 180; // Default value
            }
            Console.WriteLine("\n\n-----------------------------  Current Schedule ----------------------------------\n\n");

            // Generate the old school schedule
            List<Day> oldShedule = OurSchoolSchedule.CreateOurSchedule();
            schedules.Add(oldShedule);
            Review();
            oldSchedulePoints = bestSchedules[0].Item2;
            PrintSchedule(oldShedule);
            Console.WriteLine("Old schedule has points: " + oldSchedulePoints);
            endallthreads = false;

            // Start the watchdog thread to terminate the program after a specified time
            Thread watchdog = new Thread(Watchdog);
            watchdog.Start();

            // Start generator and review threads
            Thread[] generatorThreads = new Thread[Environment.ProcessorCount / 2];
            Thread[] reviewThreads = new Thread[Environment.ProcessorCount / 2];
            for (int i = 0; i < generatorThreads.Length; i++)
            {
                generatorThreads[i] = new Thread(Generator);
                generatorThreads[i].Start();
            }
            for (int i = 0; i < reviewThreads.Length; i++)
            {
                reviewThreads[i] = new Thread(Review);
                reviewThreads[i].Start();
            }

            // Wait for generator and review threads to finish
            foreach (var thread in generatorThreads)
            {
                thread.Join();
            }

            foreach (var thread in reviewThreads)
            {
                thread.Join();
            }

            // Wait for the watchdog thread to finish
            watchdog.Join();

            // Output the results
            try
            {            
                Console.WriteLine("\n\n" + "-----------------------------  Best schedules ------------------------------------" +
                    "\n\n\nBest schedule #1");
                PrintSchedule(bestSchedules[0].Item1);
                Console.WriteLine("#1 schedule has " + bestSchedules[0].Item2 + " points, which means it has " + (bestSchedules[0].Item2 - oldSchedulePoints) + " more points\n");

                Console.WriteLine("\nBest schedule #2");
                PrintSchedule(bestSchedules[1].Item1);
                Console.WriteLine("#2 schedule has " + bestSchedules[1].Item2 + " points, which means it has " + (bestSchedules[1].Item2 - oldSchedulePoints) + " more points\n");

                Console.WriteLine("\nBest schedule #3");
                PrintSchedule(bestSchedules[2].Item1);
                Console.WriteLine("#3 schedule has " + bestSchedules[2].Item2 + " points, which means it has " + (bestSchedules[2].Item2 - oldSchedulePoints) + " more points\n");

                Console.WriteLine("\n\n" + "--------------------------------- Results ----------------------------------------" +
                    "\n\nTotal schedules processed: " + schedulesCount);
                Console.WriteLine("Total reviewed processed: " + reviewedCount);
                Console.WriteLine("Total better schedules: " + betterSchedules);
                Console.WriteLine("\n----------------------------------------------------------------------------------");

                Console.WriteLine("Stiskněte ENTER pro ukončení programu");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error happened: " + ex.Message);
            }

            
        }

        /// <summary>
        /// Creates a Day instance with a generated timetable based on the provided subjects, total hours, and empty hours.
        /// </summary>
        /// <param name="subjects">The list of subjects available for scheduling.</param>
        /// <param name="dayName">The name of the day (e.g., "Monday", "Tuesday").</param>
        /// <param name="hours">The total hours available for the day.</param>
        /// <param name="emptyhours">The number of empty hours in the schedule.</param>
        /// <returns>A Day instance with a generated timetable.</returns>
        static Day CreateDay(List<Subject> subjects, string dayName, int hours, int emptyhours)
        {
            Day d = new Day(subjects, hours, emptyhours);
            d.GenerateTimetable();

            //PrintSchedule(d, dayName);

            return d;
        }

        /// <summary>
        /// Creates a week schedule by generating days with random hour distribution based on the available subjects.
        /// </summary>
        /// <returns>A list of Day instances representing a week schedule.</returns>
        static List<Day> CreateWeek()
        {
            var subjects = JsonSerializer.Deserialize<List<Subject>>(json);

            List<Day> week = new List<Day>();
            int x = 10;

            int a = random.Next(minValue: 6, maxValue: x);
            x = CalculateDayMaxHours(a, x);
            int b = random.Next(minValue: 6, maxValue: x);
            x = CalculateDayMaxHours(b, x);
            int c = random.Next(minValue: 6, maxValue: x);
            x = CalculateDayMaxHours(c, x);
            int d = random.Next(minValue: 6, maxValue: x);
            int e = 34 - (a + b + c + d);

            week.Add(CreateDay(subjects, "Monday", a, 10 - a));
            week.Add(CreateDay(subjects, "Tuesday", b, 10 - b));
            week.Add(CreateDay(subjects, "Wednesday", c, 10 - c));
            week.Add(CreateDay(subjects, "Thursday", d, 10 - d));
            week.Add(CreateDay(subjects, "Friday", e, 10 - e));

            return week;
        }


        /// <summary>
        /// This method is used to make the school schedule hours divided into all days, so that friday is not without hours
        /// </summary>
        static int CalculateDayMaxHours(int number, int x)
        {
            //This condition is to make next day with less hours, beacuse this day was longer then average
            if (number >= x - 2 && x - 2 >= 6)
            {
                x -= 2;
                return x;
            }
            else if (x < 9)
            {
                x += 2;
                return x;
            }
            //Second condition is to make next day with more hours, beacuse this day was shorter then average
            //Also this condition makes sure that the day is not longer then 10 hours

            //If the day is averagly long then nothing changes
            return x;
        }

        /// <summary>
        /// Continuously generates new schedules and adds them to a shared list until a termination condition is met.
        /// </summary>
        static void Generator()
        {
            while (!endallthreads)
            {
                // Create a new schedule for the week
                List<Day> newSchedule = CreateWeek();

                // Ensure thread safety when accessing shared resources
                lock (lockObject)
                {
                    // Add the new schedule to the shared list
                    schedules.Add(newSchedule);

                    // Increment the count of generated schedules
                    schedulesCount++;
                }
            }
        }

        /// <summary>
        /// Review makes the list with the top 3 schedules, it firstly puts the schedule in Reviewer to get back its
        /// reviewed points and then based on those points it could be saved
        /// </summary>
        static void Review()
        {
            List<Day> currentSchedule = null;
            int points = 0;

            while (!endallthreads)
            {
                lock (lockObject)
                {
                    if (schedules.Count > 0)
                    {
                        currentSchedule = schedules[0];
                        schedules.RemoveAt(0);
                        reviewedCount++;
                    }
                }

                if (currentSchedule != null)
                {
                    points = reviewer.ReviewWeek(currentSchedule);

                    lock (lockObject)
                    {
                        if (bestSchedules != null)
                        {
                            if (bestSchedules.Count > 2)
                            {
                                if (points > bestSchedules[0].Item2)
                                {
                                    bestSchedules.Insert(0, (currentSchedule, points));
                                }
                                if (points < bestSchedules[0].Item2 && points > bestSchedules[1].Item2)
                                {
                                    bestSchedules.Insert(1, (currentSchedule, points));
                                }
                                if (points < bestSchedules[1].Item2 && points > bestSchedules[2].Item2)
                                {
                                    bestSchedules.Insert(2, (currentSchedule, points));
                                }

                                if (points > oldSchedulePoints)
                                {
                                    betterSchedules++;
                                }

                                if (bestSchedules.Count > 3)
                                {
                                    bestSchedules.RemoveAt(3);
                                }
                            }
                            else
                            {
                                bestSchedules.Add((currentSchedule, points));
                                if (bestSchedules.Count == 1) { endallthreads = true; }  
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// PrintSchedule method is used to visualize the schedule in more understandable way
        /// </summary>
        /// <param name="d"></param>
        static void PrintSchedule(List<Day> d)
        {
            Console.WriteLine("+------------- 1 ---- 2 ---- 3 ---- 4 ---- 5 ---- 6 ---- 7 ---- 8 ---- 9 --- 10 -+");

            int dayCount = 0;
            int i = 0;
            foreach (Day day in d)
            {
                List<Lesson> dailySchedule = day.GetDailySchedule();
                
                switch (dayCount)
                {
                    case 0:
                        Console.Write("| Monday    ");
                        break;
                    case 1:
                        Console.Write("| Tuesday   ");
                        break;
                    case 2:
                        Console.Write("| Wednesday ");
                        break;
                    case 3:
                        Console.Write("| Thursday  ");
                        break;
                    case 4:
                        Console.Write("| Friday    ");
                        break;
                }
                dayCount++;
                
                foreach (Lesson l in dailySchedule)
                {
                    

                    if (l == null)
                    {
                        Console.Write("|  -  |");
                        continue;
                    }

                    Console.Write("| "+l.SubjectName+ " |");
                }
                i = 0;
                if (dailySchedule.Count < 10)
                {
                    for (int j = 0; j < 10 - dailySchedule.Count; j++)
                    {
                        Console.Write("|  -  |");
                    }
                }
                Console.WriteLine();

            }
            Console.WriteLine("+--------------------------------------------------------------------------------+");
        }

        /// <summary>
        /// Watchdog makes sure to stop all threads when the timer ends
        /// </summary>
        static void Watchdog()
        {
            Thread.Sleep(time*1000);
            endallthreads = true;
        }
    }
}