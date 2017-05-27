using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT321_Assignment_3_WPF
{
    class Unit
    {
        public int ID {get; set;}
        public string Code {get; set;}
        public DateTime Year {get; set;}
        public int Trimester {get; set;}
        public int TotalLectures {get; set;}
        public int TotalPracticals {get; set;}

        // constructor
        public Unit(string code, DateTime year, int tri, int tlec, int tprac)
        {
            Code = code;
            Year = year;
            Trimester = tri;
            TotalLectures = tlec;
            TotalPracticals = tprac;
        }
    }
    
    class Assessment
    {
        private int AssessmentID;
        private string Name;
        private int TotalMarks;
        private decimal Weight;

        // constructor
        public Assessment(string name, int tmark, decimal weight)
        {
            Name = name;
            TotalMarks = tmark;
            Weight = weight;
        }
    }
}
