using System;

namespace SARMS.Content
{
    public class Assessment : IEquatable<Assessment>
    {
        #region Private Fields
        private int _assessmentID;
        private string _name;
        private int _totalMarks;
        private decimal _weight;
        private Unit _unit;
        #endregion

        #region Getters and Setters
        public int AssessmentID
        {
            get { return _assessmentID; }
            set { _assessmentID = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int TotalMarks
        {
            get { return _totalMarks; }
            set { _totalMarks = value; }
        }
        public decimal Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        public Unit unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as Assessment;
            if (other != null) return Equals(other);
            else return false;
        }

        public bool Equals(Assessment other)
        {
            if (other == null) return false;
            if (other.AssessmentID != this.AssessmentID) return false;
            return true;
        }

        // constructor
        public Assessment(int id, string name, int totalMark, decimal weight, Unit unit)
        {
            _assessmentID = id;
            _name = name;
            _totalMarks = totalMark;
            _weight = weight;
            _unit = unit;
        }
    }
}
