using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// This class is used to create lessons in generator and json deserialization.
    /// </summary>
    public class Lesson
    {
        /// <summary>
        /// This constructor is used in the generator
        /// </summary>
        public Lesson(string subjectName, string teacher, string classs, string typeOfLecture, string floor)
        {
            SubjectName = subjectName;
            Teacher = teacher;
            Class = classs;
            TypeOfLecture = typeOfLecture;
            Floor = floor;
        }

        /// <summary>
        /// This constructor is used by the OurSchoolSchedule class
        /// </summary>
        public Lesson()
        {
        }

        public string SubjectName { get; set; }
        public string Teacher { get; set; }
        public int Hours { get; set; }
        public string Class { get; set; }
        public string TypeOfLecture { get; set; }
        public string Floor { get; set; }

        public override string? ToString()
        {
            return $"| {SubjectName} | {Teacher} | Class: {Class} | {TypeOfLecture} | Floor: {Floor} |"; ;
        }
    }
}
