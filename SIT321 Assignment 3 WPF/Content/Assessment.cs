using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT321_Assignment_3_WPF.Content
{
    class Assessment
    {
        private int AssessmentID;
        private string Name;
        private int TotalMarks;
        private decimal Weight;

        // constructor
        public Assessment(int id, string name, int tmark, decimal weight)
        {
            AssessmentID = id;
            Name = name;
            TotalMarks = tmark;
            Weight = weight;
        }
    }
}
