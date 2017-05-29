using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT321_Assignment_3_WPF.Content
{
    class Unit
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
