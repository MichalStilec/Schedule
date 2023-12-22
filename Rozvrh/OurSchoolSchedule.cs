using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// Is used to create any schedule from the json file.
    /// </summary>
    public static class OurSchoolSchedule
    {
        /// <summary>
        /// Creates the school schedule based on the data stored in a JSON file.
        /// </summary>
        /// <returns>A list of days, each containing a schedule of lessons</returns>
        internal static List<Day> CreateOurSchedule()
        {
            // Path to the JSON file containing lesson data
            string filePath = "JsonFiles/oldschedule.json";

            // List to store deserialized lesson objects
            List<Lesson> lessons;

            // Deserialize the JSON file into a list of Lesson objects
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                lessons = JsonSerializer.Deserialize<List<Lesson>>(json);
            }

            // Split the list of lessons into daily lessons
            List<List<Lesson>> dailyLessons = new List<List<Lesson>>
        {
            lessons.Take(10).ToList(),
            lessons.Skip(10).Take(10).ToList(),
            lessons.Skip(20).Take(10).ToList(),
            lessons.Skip(30).Take(10).ToList(),
            lessons.Skip(40).Take(10).ToList()
        };

            // Create a list of Day objects based on the daily lessons
            List<Day> days = dailyLessons.Select(lessonsOfDay => new Day(lessonsOfDay)).ToList();

            // Assign the daily schedule to each Day object
            for (int i = 0; i < days.Count; i++)
            {
                days[i].dailySchedule = dailyLessons[i];
            }

            // Return the list of days with their respective schedules
            return days;
        }
    }

}
