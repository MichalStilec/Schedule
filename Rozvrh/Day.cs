using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// Represents a day in the school schedule, containing a daily timetable of lessons.
    /// </summary>
    internal class Day
    {
        private List<Subject> subjects;
        private int totalHours;
        private int emptyhours;
        private string previousSubject = "";
        private string previousType = "";

        /// <summary>
        /// Gets or sets the daily schedule of lessons.
        /// </summary>
        public List<Lesson> dailySchedule;

        /// <summary>
        /// Initializes a new instance of the Day class with specified subjects, total hours, and empty hours.
        /// </summary>
        /// <param name="subjects">The list of subjects for the day.</param>
        /// <param name="totalHours">The total hours available for the daily schedule.</param>
        /// <param name="emptyhours">The number of empty hours in the schedule.</param>
        public Day(List<Subject> subjects, int totalHours, int emptyhours)
        {
            this.subjects = subjects;
            this.totalHours = totalHours;
            this.emptyhours = emptyhours;
            this.dailySchedule = new List<Lesson>();
        }

        /// <summary>
        /// Initializes a new instance of the Day class with a pre-defined daily schedule.
        /// </summary>
        /// <param name="dailySchedule">The pre-defined daily schedule of lessons.</param>
        public Day(List<Lesson> dailySchedule)
        {
            this.dailySchedule = new List<Lesson>();
        }

        /// <summary>
        /// This is the whole generator, that is used for creating all the schedules
        /// </summary>
        public void GenerateTimetable()
        {
            Random random = new Random();
            int currentHours = 0;
            int hours = 0;
            foreach (var c in subjects) 
            {
                hours += c.Hours;
            }
            if (totalHours > hours)
            {
                totalHours = hours;
            }

            int tries = 0;
            while (currentHours < totalHours)
            {

                int courseIndex = random.Next(subjects.Count);
                var course = subjects[courseIndex]; 


                int classIndex = random.Next(course.Class.Count);
                string selectedClass = course.Class[classIndex];

                // This statement, that checks if the generator would want to add a Excercise to a end of the day (last hour),
                // but because Excercises have 2 hours, then it would do some problems, so the generator has three tries to do
                // add some other Lesson, but if the is not other choice, then it leaves the day as it is 
                if (course.TypeOfLecture == "Excercise" && currentHours + 1 == totalHours && tries < 3)
                {
                    tries++;
                    continue;
                }
                else if (course.TypeOfLecture == "Excercise" && currentHours + 1 == totalHours && tries == 3)
                {
                    break;
                }

                // This checks if the course is Excercise, if so, then the generator adds the lesson twice and also removes two hours
                // Otherwise, it just adds one lesson and deletes one hour
                if (course.TypeOfLecture == "Excercise")
                {
                    Lesson scheduleEntry = new Lesson(course.SubjectName, course.Teacher, selectedClass, course.TypeOfLecture, course.Floor[classIndex]);
                    dailySchedule.Add(scheduleEntry);
                    dailySchedule.Add(scheduleEntry);

                    currentHours += 2;
                    course.Hours -= 2;
                }
                else
                {
                    Lesson scheduleEntry = new Lesson(course.SubjectName, course.Teacher, selectedClass, course.TypeOfLecture, course.Floor[classIndex]);
                    dailySchedule.Add(scheduleEntry);

                    currentHours += 1;
                    course.Hours--;
                }

                // If the course doesnt have any more hours left, then it gets deleted
                if (course.Hours == 0)
                {
                    subjects.RemoveAt(courseIndex);
                }
            }

            // This does all the empty hours, that are (after the creation of the schedule) randomly inserted between all lessons
            while (emptyhours > 0)
            {
                dailySchedule.Insert(random.Next(dailySchedule.Count()), null);
                emptyhours--;
            }
        }

        /// <summary>
        /// Gets the daily schedule of lessons.
        /// </summary>
        /// <returns>The list of lessons representing the daily schedule.</returns>
        public List<Lesson> GetDailySchedule()
        {
            return dailySchedule;
        }
    }

    
}
