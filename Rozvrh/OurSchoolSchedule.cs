using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class OurSchoolSchedule
    {
        internal static List<Day> CreateOurSchedule()
        {
            List<Day> days = new List<Day>();

            List<Lesson> monday = new List<Lesson>();

            monday.Add(new Lesson("WA ", "Mgr. Jan Pavlat & Mgr. Nykyta Narusevych", "17b", "Excercise", "3"));
            monday.Add(new Lesson("WA ", "Mgr. Jan Pavlat & Mgr. Nykyta Narusevych", "17b", "Excercise", "3"));
            monday.Add(new Lesson("C  ", "MUDr. Kristina Studenkova", "24", "Theory", "4"));
            monday.Add(new Lesson("A  ", "Ing. Bc. Sarka Paltikova", "24", "Theory", "4"));
            monday.Add(new Lesson("M  ", "Mgr. Eva Neugebauerova", "24", "Theory", "4"));
            monday.Add(null);
            monday.Add(new Lesson("PV ", "Mgr. Alena Reichlova & Ing. Ondrej Mandik", "18b", "Excercise", "3"));
            monday.Add(new Lesson("PV ", "Mgr. Alena Reichlova & Ing. Ondrej Mandik", "18b", "Excercise", "3"));
            monday.Add(null);
            monday.Add(null);

            days.Add(new Day(monday));
            days[0].dailySchedule = monday;


            List<Lesson> tuesday = new List<Lesson>();

            tuesday.Add(new Lesson("M  ", "Mgr. Eva Neugebauerova", "24", "Theory", "4"));
            tuesday.Add(new Lesson("TP ", "Ing. Vit Nohejl", "24", "Theory", "4"));
            tuesday.Add(new Lesson("PSS", "Jan Molic", "8a", "Excercise", "2"));
            tuesday.Add(new Lesson("PSS", "Jan Molic", "8a", "Excercise", "2"));
            tuesday.Add(new Lesson("A  ", "Ing. Bc. Sarka Paltikova", "5b", "Theory", "4"));
            tuesday.Add(new Lesson("AM ", "Ing. Filip Kallmunzer", "24", "Theory", "4"));
            tuesday.Add(null);
            tuesday.Add(new Lesson("TV ", "Mgr. Pavel Lopocha", "TV", "Physical Excercise", "0"));
            tuesday.Add(null);
            tuesday.Add(null);

            days.Add(new Day(tuesday));
            days[1].dailySchedule = tuesday;


            List<Lesson> wednesday = new List<Lesson>();

            wednesday.Add(new Lesson("PIS", "Ing. Lucie Brcakova", "24", "Theory", "4"));
            wednesday.Add(new Lesson("C  ", "MUDr. Kristina Studenkova", "24", "Theory", "4"));
            wednesday.Add(new Lesson("CIT", "Ing. Mgr. Vladimir Vana", "19c", "Excercise", "3"));
            wednesday.Add(new Lesson("CIT", "Ing. Mgr. Vladimir Vana", "19c", "Excercise", "3"));
            wednesday.Add(new Lesson("AM ", "Ing. Filip Kallmunzer", "24", "Theory", "4"));
            wednesday.Add(new Lesson("M  ", "Mgr. Eva Neugebauerova", "24", "Theory", "4"));
            wednesday.Add(new Lesson("DS ", "Ing. Ivana Kantnerova", "24", "Theory", "4"));
            wednesday.Add(null);
            wednesday.Add(null);
            wednesday.Add(null);

            days.Add(new Day(wednesday));
            days[2].dailySchedule = wednesday;


            List<Lesson> thursday = new List<Lesson>();

            thursday.Add(new Lesson("WA ", "Mgr. Jan Pavlat", "24", "Theory", "4"));
            thursday.Add(new Lesson("M  ", "Mgr. Eva Neugebauerova", "24", "Theory", "4"));
            thursday.Add(new Lesson("PIS", "Ing. Lucie Brcakova", "24", "Theory", "4"));
            thursday.Add(new Lesson("PV ", "Mgr. Alena Reichlova", "24", "Theory", "4"));
            thursday.Add(new Lesson("A  ", "Ing. Bc. Sarka Paltikova", "24", "Theory", "4"));
            thursday.Add(new Lesson("C  ", "MUDr. Kristina Studenkova", "24", "Theory", "4"));
            thursday.Add(new Lesson("PSS", "Ing. Lukas Masopust", "24", "Theory", "4"));
            thursday.Add(null);
            thursday.Add(null);
            thursday.Add(null);

            days.Add(new Day(thursday));
            days[3].dailySchedule = thursday;


            List<Lesson> friday = new List<Lesson>();

            friday.Add(null);
            friday.Add(new Lesson("PIS", "Ing. Lucie Brcakova & Ing. Vit Nohejl", "19b", "Excercise", "3"));
            friday.Add(new Lesson("PIS", "Ing. Lucie Brcakova & Ing. Vit Nohejl", "19b", "Excercise", "3"));
            friday.Add(new Lesson("A  ", "Ing. Bc. Sarka Paltikova", "29", "Theory", "4"));
            friday.Add(new Lesson("TV ", "Mgr. Pavel Lopocha", "TV", "Physical Excercise", "0"));
            friday.Add(new Lesson("DS ", "Ing. Ivana Kantnerova", "17b", "Excercise", "3"));
            friday.Add(new Lesson("DS ", "Ing. Ivana Kantnerova", "17b", "Excercise", "3"));
            friday.Add(null);
            friday.Add(null);
            friday.Add(null);

            days.Add(new Day(friday));
            days[4].dailySchedule = friday;


            return days;



            //string filePath = "rozvrhpriklad.json"; // Replace with the actual path to your JSON file

            //List<Lesson> lessons = new List<Lesson>();
            //List<Day> days = new List<Day>();
            //int i = 0;
            //using (StreamReader r = new StreamReader(filePath))
            //{
            //    string json = r.ReadToEnd();
            //    lessons = JsonSerializer.Deserialize<List<Lesson>>(json);
            //    Console.WriteLine(lessons.Count);
            //    if (lessons.Count == 10)
            //    {
            //        days.Add(new Day(lessons));
            //        days[i].dailySchedule = lessons;
            //        lessons.Clear();
            //        i++;
            //    }
            //}


            //return days;
        }
    }
}
