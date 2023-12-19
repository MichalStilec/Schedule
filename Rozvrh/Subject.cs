using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Subject
    {

        public string SubjectName { get; set; }
        public string Teacher { get; set; }
        public int Hours { get; set; }  
        public List<string> Class { get; set; }
        public string TypeOfLecture { get; set; }
        public List<string> Floor { get; set; }
    }
}
