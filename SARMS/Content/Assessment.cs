using System;

namespace SARMS.Content
{
    public class Assessment : IEquatable<Assessment>
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

        public override bool Equals(object obj)
        {
            var a = obj as Assessment;
            return Equals(a);
        }

        public bool Equals(Assessment a)
        {
            if (a != null)
            {
                if (this.AssessmentID == a.AssessmentID)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
