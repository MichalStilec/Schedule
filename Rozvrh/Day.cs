using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Day
    {
        private List<Subject> subjects;
        private int totalHours;
        private int emptyhours;
        private string previousSubject = "";
        private string previousType = "";

        public List<Lesson> dailySchedule;

        public Day(List<Subject> subjects, int totalHours, int emptyhours)
        {
            this.subjects = subjects;
            this.totalHours = totalHours;
            this.emptyhours = emptyhours;
            this.dailySchedule = new List<Lesson>();
        }

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


                if (course.SubjectName == previousSubject && course.TypeOfLecture == previousType && previousType != "-" && tries <= 5)
                {
                    tries++;
                    continue;
                }
                else
                {
                    previousSubject = course.SubjectName;
                    previousType = course.TypeOfLecture;
                }

                if (currentHours == totalHours - 1 && course.TypeOfLecture == "Excercise")
                {
                    continue;
                }


                int classIndex = random.Next(course.Class.Count);
                string selectedClass = course.Class[classIndex];

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


                if (course.Hours == 0)
                {
                    subjects.RemoveAt(courseIndex);
                }
            }

            while (emptyhours > 0)
            {
                dailySchedule.Insert(random.Next(dailySchedule.Count()), null);
                emptyhours--;
            }
        }

        public List<Lesson> GetDailySchedule()
        {
            return dailySchedule;
        }
    }
}
