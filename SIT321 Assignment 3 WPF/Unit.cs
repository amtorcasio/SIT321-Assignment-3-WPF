using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT321_Assignment_3_WPF
{
    class Unit
    {
        private int ID;
        private string Code;
        private DateTime Year;
        private int Trimester;
        private int TotalLectures;
        private int TotalPracticals;

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
