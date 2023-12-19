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
        static int time = 25;
        static bool endallthreads = false;
        static int schedules = 0;
        static object lockObject = new object();

        static void Main(string[] args)
        {
            Thread watchdog = new Thread(Watchdog);
            watchdog.Start();

            Thread[] threads = new Thread[10];
            for (int i = 0; i < threads.Length; i++) 
            {
                threads[i] = new Thread(Generator);
                threads[i].Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            endallthreads = true;
            watchdog.Join();

            Console.WriteLine($"\nTotal schedules processed: {schedules}");

        }
        static void CreateDay(List<Subject> subjects, string dayName, int hours, int emptyhours)
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

        static void CreateWeek()
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

        static void Generator()
        {
            while (!endallthreads)
            {
                if (schedules % 1000 == 0) { Console.WriteLine(schedules); }
                CreateWeek();
                lock(lockObject)
                {
                    schedules++;
                }
            }
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

        static void Watchdog()
        {
            Thread.Sleep(time*1000);
            endallthreads = true;
        }
    }
}