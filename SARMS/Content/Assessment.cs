using System;
using System.ComponentModel;

namespace SARMS.Content
{
    public class Assessment : IEquatable<Assessment>, INotifyPropertyChanged
    {
        #region Private Fields
        private long _assessmentID;
        private string _name;
        private int _totalMarks;
        private double _weight;
        private Unit _unit;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Getters and Setters
        public long AssessmentID
        {
            get { return _assessmentID; }
            set { _assessmentID = value; }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public int TotalMarks
        {
            get { return _totalMarks; }
            set
            {
                _totalMarks = value;
                OnPropertyChanged("TotalMarks");
            }
        }
        public double Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnPropertyChanged("Weight");
            }
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // constructor
        public Assessment(long id, string name, int totalMark, double weight, Unit unit)
        {
            _assessmentID = id;
            _name = name;
            _totalMarks = totalMark;
            _weight = weight;
            _unit = unit;
        }

        public Assessment(string name, int totalMark, double weight, Unit unit)
        {
            _name = name;
            _totalMarks = totalMark;
            _weight = weight;
            _unit = unit;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
