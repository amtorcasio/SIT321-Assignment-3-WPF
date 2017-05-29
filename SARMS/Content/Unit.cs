using System;
using System.Collections.Generic;

namespace SARMS.Content
{
    public class Unit
    {
        public int ID {get; set;}
        public string Code {get; set;}
        public DateTime Year {get; set;}
        public int Trimester {get; set;}
        public int TotalLectures {get; set;}
        public int TotalPracticals {get; set;}

        public List<Assessment> Assessments;

        // constructor
        public Unit(string code, DateTime year, int tri, int tlec, int tprac)
        {
            Code = code;
            Year = year;
            Trimester = tri;
            TotalLectures = tlec;
            TotalPracticals = tprac;
            Assessments = new List<Assessment>();
        }
    }

    // moved Assessment class to respective .cs file
}
