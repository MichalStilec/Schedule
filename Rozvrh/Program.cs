using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace ConsoleApp1
{
    public class Program
    {
        static Random random = new Random();
        static string json = new StreamReader("data.json").ReadToEnd();
        static List<List<Day>> schedules = new List<List<Day>>();
        static Dictionary<List<Day>, int> bestSchedules = new Dictionary<List<Day>, int>();
        static int time = 15;
        static bool endallthreads = false;
        static int schedulesCount = 0;
        static int reviewedCount = 0;
        static object lockObject = new object();

        static void Main(string[] args)
        {
            List<Day> oldshedule = OurSchoolSchedule.CreateOurSchedule();
            schedules.Add(oldshedule);
            Review();
            KeyValuePair<List<Day>, int> oldSchoolSchedule = bestSchedules.First();
            int oldSchoolSchedulePoints = oldSchoolSchedule.Value;
            Console.WriteLine("Our current schedule has points: " + oldSchoolSchedulePoints);
            endallthreads = false;


            Thread watchdog = new Thread(Watchdog);
            watchdog.Start();

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
            foreach (var thread in generatorThreads)
            {
                thread.Join();
            }

            foreach (var thread in reviewThreads)
            {
                thread.Join();
            }


            watchdog.Join();

            Console.WriteLine($"\nTotal schedules processed: {schedulesCount}");
            Console.WriteLine($"Total reviewed processed: {reviewedCount}");

            KeyValuePair<List<Day>, int> firstScheduleEntry = bestSchedules.First();
            List<Day> firstSchedule = firstScheduleEntry.Key;
            PrintSchedule(firstSchedule);


            if (bestSchedules.Keys.Any())
            {
                Console.WriteLine(bestSchedules.Values.First());
            }
            else
            {
                Console.WriteLine("No schedules found in bestSchedules.");
            }

        }
        static Day CreateDay(List<Subject> subjects, string dayName, int hours, int emptyhours)
        {
            Day d = new Day(subjects, hours, emptyhours);
            d.GenerateTimetable();

            //PrintSchedule(d, dayName);

            return d;
        }

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

        static int CalculateDayMaxHours(int number, int x)
        {
            // This method is used to make the school schedule hours divided into all days, so that friday is not without hours

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

        static void Generator()
        {
            while (!endallthreads)
            {
                List<Day> newSchedule = CreateWeek();
                lock (lockObject)
                {
                    schedules.Add(newSchedule);
                    schedulesCount++;
                }
            }
        }

        static void Review()
        {
            Reviewer reviewer = new Reviewer();
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
                            if (bestSchedules.Count > 0)
                            {
                                KeyValuePair<List<Day>, int> firstScheduleEntry = bestSchedules.First();
                                List<Day> firstSchedule = firstScheduleEntry.Key;
                                int savedPoints = firstScheduleEntry.Value;

                                if (points > savedPoints)
                                {
                                    bestSchedules.Remove(firstSchedule);
                                    bestSchedules.Add(currentSchedule, points);
                                }
                            }
                            else
                            {
                                bestSchedules.Add(currentSchedule, points);
                                endallthreads = true;
                            }
                        }
                    }
                }
            }
        }

        static void PrintSchedule(List<Day> d)
        {
            int dayCount = 0;
            int i = 0;
            foreach (Day day in d)
            {
                List<Lesson> dailySchedule = day.GetDailySchedule();
                Console.WriteLine("------------------------------------------------------------------------------------------");
                
                switch (dayCount)
                {
                    case 0:
                        Console.WriteLine("Monday");
                        break;
                    case 1:
                        Console.WriteLine("Tuesday");
                        break;
                    case 2:
                        Console.WriteLine("Wednesday");
                        break;
                    case 3:
                        Console.WriteLine("Thursday");
                        break;
                    case 4:
                        Console.WriteLine("Friday");
                        break;
                }
                dayCount++;
                foreach (Lesson l in dailySchedule)
                {
                    i++;
                    if (i < 10)
                    {
                        Console.Write(i + ".  ");
                    }
                    else { Console.Write(i + ". "); }

                    
                    if (l == null)
                    {
                        Console.WriteLine("|  -  |");
                        continue;
                    }

                    Console.WriteLine(l);

                    if (dailySchedule.Count == 9) 
                    {
                        if (i == 9)
                        {
                            Console.WriteLine("10. |  -  |");
                        }
                    }
                }
                i = 0;
            }
                
        }
        static void Watchdog()
        {
            Thread.Sleep(time*1000);
            endallthreads = true;
        }
    }
}