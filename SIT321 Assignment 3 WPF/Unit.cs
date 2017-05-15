using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT321_Assignment_3_WPF
{
    class Unit
    {
        public int ID;
        public string Code;
        public DateTime Year;
        public int Trimester;
        public int TotalLectures;
        public int TotalPracticals;
    }
    
    class Assessment
    {
        public int AssessmentID;
        public string Name;
        public int TotalMarks;
        public decimal Weight;
    }
}
