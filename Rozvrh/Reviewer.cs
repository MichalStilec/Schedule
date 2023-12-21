using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Reviewer
    {
        public int TotalPoints { get; private set; }
        public Reviewer()
        {

        }

        
        public int ReviewWeek(List<Day> week)
        {
            int points = 0;
            foreach (Day day in week)
            {
                points += ReviewDay(day);
            }
            return points;
        }

        public int ReviewDay(Day d)
        {
            int points = 0;
            points += Rating1(d);
            points += Rating2(d);
            points += Rating3(d);
            points += Rating4(d);
            points += Rating5(d);
            points += Rating6(d);
            points += Rating7(d);
            points += Rating8(d);
            points += Rating9(d);
            points += Rating10(d);
            return points;
        }

        /// <summary>
        /// If the school starts one hour later, then based on my preference it is better and in result schedule gets plus points.
        /// On the other side, if the school is long, schedule gets minus points.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <summary>
        /// If the school starts one hour later, then based on my preference it is better and in result schedule gets plus points.
        /// On the other side, if the school is long, schedule gets minus points.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating1(Day d)
        {
            int points = 0;

            for (int i = 0; i <= d.dailySchedule.Count; i++)
            {
                if (i == 0 && d.dailySchedule[i] == null)
                {
                    points += 300;
                }
                if (i >= 8 && i < d.dailySchedule.Count && d.dailySchedule[i] != null)
                {
                    points -= 100;
                }
            }

            return points;
        }

        /// <summary>
        /// If it is the same course multiple times in one day and it is not a multi-hour class, it is wrong.
        /// Practice and theory can be in one day.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating2(Day d)
        {
            List<string> subjectNames = new List<string>();
            int points = 0;

            foreach (Lesson l in d.dailySchedule)
            {
                if (l != null && l.TypeOfLecture != "Excercise")
                {
                    if (subjectNames.Contains(l.SubjectName))
                    {
                        points -= 100;
                        break;
                    }
                    else
                    {
                        subjectNames.Add(l.SubjectName);
                    }
                }
            }
            return points;
        }

        /// <summary>
        /// If I have to move to a different floor between classes it's bad, if to a different classroom it's also
        /// bad but if it's on the same floor it's not so bad.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating3(Day d)
        {
            int points = 0;
            string previousClass = "24";
            string previousFloor = "4";

            foreach (Lesson l in d.dailySchedule)
            {
                if (l != null)
                {
                    if (l.Floor == previousFloor && l.Class == previousClass)
                    {
                        points += 100;
                    }
                    else if (l.Floor == previousFloor && l.Class != previousClass)
                    {
                        points += 50;
                    }
                    else
                    {
                        points -= 50;
                    }

                    previousClass = l.Class;
                    previousFloor = l.Floor;
                }
            }

            return points;
        }

        /// <summary>
        /// Lunch is served between 5 and 8, so each day one of the hours 5, 6, 7 or 8 must be free for lunch.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating4(Day d)
        {
            int points = 0;

            for (int i = 4; i < d.dailySchedule.Count; i++)
            {
                if ((d.dailySchedule[i]) == null)
                {
                    points += 200;
                    break;
                }
            }

            return points;
        }

        /// <summary>
        /// You should ideally study 5-6 hours a day, more than that is wrong, 8 is the ceiling, 9 is a problem and 10 is probably not even legal.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating5(Day d)
        {
            int points = 0;
            int hours = 0;


            foreach (Lesson l in d.dailySchedule)
            {
                if (l != null) { hours++; }
            }

            if (hours <= 6)
            {
                points += 500;
            }
            if (hours == 8)
            {
                points -= 200;
            }
            if (hours == 9)
            {
                points -= 500;
            }
            if (hours == 10)
            {
                points -= 2000;
            }

            return points;
        }

        /// <summary>
        /// When the exercise is two hours, the hours have to be together on the same day.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating6(Day d)
        {
            int points = 0;
            string previousSubject = "";

            foreach (Lesson l in d.dailySchedule)
            {
                if (l != null && l.TypeOfLecture == "Excercise" && l.SubjectName == previousSubject)
                {
                    points += 500;
                }
                if (l == null)
                {
                    previousSubject = "";
                }
                else
                {
                    previousSubject = l.SubjectName;
                }

            }

            return points;
        }

        /// <summary>
        /// Maths and profile subjects should not be taught in the first hour or after the lunch break, points must be deducted for this.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating7(Day d)
        {
            int points = 0;
            for (int i = 0; i < d.dailySchedule.Count; i++)
            {
                
                if (i == 0 && d.dailySchedule[i] != null && (d.dailySchedule[i].SubjectName == "A  " || d.dailySchedule[i].SubjectName == "C  " || d.dailySchedule[i].SubjectName == "M  "))
                {
                    points -= 500;
                }
                if (i >= 6 && d.dailySchedule[i] != null && (d.dailySchedule[i].SubjectName == "A  " || d.dailySchedule[i].SubjectName == "C  " || d.dailySchedule[i].SubjectName == "M  "))
                {
                    points -= 500;
                }
            }

            return points;
        }

        /// <summary>
        /// This rating checks if there are three excercise hours in one day which would be demanding for the students 
        /// so that will do minus points for the schedule
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating8(Day d)
        {
            int points = 0;
            int numberOfExcercises = 0;
            foreach (Lesson l in d.dailySchedule)
            {
                if (l != null && l.TypeOfLecture == "Excercise")
                {
                    numberOfExcercises++;
                }
            }
            if (numberOfExcercises >= 3) 
            { 
                points -= 1000;
            }

            return points;
        }


        /// <summary>
        /// This rating check if the day schedule doesnt have the hours like this:  hour - emptyhour - hour - emptyhour - hour.
        /// This means that students would have two waiting hours during the day, which would lose precious student time
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating9(Day d)
        {
            int points = 0;

            for (int i = 0; i < d.dailySchedule.Count - 4; i++)
            {
                if (d.dailySchedule[i] != null && d.dailySchedule[i+1] == null && d.dailySchedule[i+2] != null && d.dailySchedule[i+3] == null && d.dailySchedule[i+4] != null)
                {
                    
                    points -= 100000;
                }
            }


            return points;
        }

        /// <summary>
        /// Wellbeing rating, which rates the day with minus points if the current day contains A, PIS, and TP subjects. On the other side if the day contains AM and TV subjects then it gives plus points.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        int Rating10(Day d)
        {
            int points = 0;

            List<string> negativeSubjects = new List<string> { "A  ", "PIS", "TP " };
            List<string> positiveSubjects = new List<string> { "AM ", "TV " };


            foreach (Lesson l in d.dailySchedule)
            {
                for (int i = 0; i < negativeSubjects.Count; i++)
                {
                    if (l != null && l.SubjectName == negativeSubjects[i])
                    {
                        negativeSubjects.Remove(negativeSubjects[i]);
                    }
                }
                for (int i = 0; i < positiveSubjects.Count; i++)
                {
                    if (l != null && l.SubjectName == positiveSubjects[i])
                    {

                        positiveSubjects.Remove(positiveSubjects[i]);
                    }
                }
            }

            if (negativeSubjects.Count == 0)
            {
                points -= 2000;
            }
            if (positiveSubjects.Count == 0)
            {
                points += 500;
            }

            return points;
        }
    }
}
