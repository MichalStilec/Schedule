using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            string json = new StreamReader("data.json").ReadToEnd();

            int schedules = 0;
            int time = 30;

            Watchdog watchdog = new Watchdog();

            Parallel.For(0, int.MaxValue, (i, loopState) =>
            {
                if (watchdog.Stopwatch.Elapsed.TotalSeconds < time)
                {
                    Console.WriteLine("Creating week");
                    CreateWeek();
                    Interlocked.Increment(ref schedules);

                    if (schedules % 10000 == 0)
                    {
                        Console.WriteLine($"Created {schedules} schedules   Time remaining {time - watchdog.Stopwatch.Elapsed.TotalSeconds}");
                    }
                }
                else
                {
                    loopState.Stop();
                    Console.WriteLine("Program stopped");
                }
            });

            Console.WriteLine($"\nTotal schedules processed: {schedules}");


            void CreateDay(List<Subject> subjects, string dayName, int hours, int emptyhours)
            {
                Day d = new Day(subjects, hours, emptyhours);
                d.GenerateTimetable();
                //List<Lesson> dailySchedule = d.GetDailySchedule();

                //Console.WriteLine("------------------------------------------------------------------------------------------");
                //Console.WriteLine(dayName);
                //int i = 0;
                //foreach (Lesson l in dailySchedule)
                //{
                //    i++;
                //    if (i < 10)
                //    {
                //        Console.Write(i + ".  ");
                //    }
                //    else { Console.Write(i + ". "); }


                //    if (l == null)
                //    {
                //        Console.WriteLine("|  -  |");
                //        continue;
                //    }
                //    Console.WriteLine(l);
                //}

                //return d;
            }

            void CreateWeek()
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

                //week.Add(CreateDay(subjects, "Monday", a, 10 - a));
                //week.Add(CreateDay(subjects, "Tuesday", b, 10 - b));
                //week.Add(CreateDay(subjects, "Wednesday", c, 10 - c));
                //week.Add(CreateDay(subjects, "Thursday", d, 10 - d));
                //week.Add(CreateDay(subjects, "Friday", e, 10 - e));

                CreateDay(subjects, "Monday", a, 10 - a);
                CreateDay(subjects, "Tuesday", b, 10 - b);
                CreateDay(subjects, "Wednesday", c, 10 - c);
                CreateDay(subjects, "Thursday", d, 10 - d);
                CreateDay(subjects, "Friday", e, 10 - e);


                //return week;
            }

            int CalculateDayMaxHours(int number, int x)
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
        }
    }
}