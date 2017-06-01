using SARMS.Content;
using SARMS.Users;
using System.ComponentModel;

namespace SARMS.Data
{
    public class StudentUnit : INotifyPropertyChanged
    {
        private Unit _unit;
        private Account _account;
        private int? _lectureAttendance;
        private int? _practicalAttendance;
        private string _staffFeedback;
        private string _studentFeedback;
        private bool? _atRisk;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Getters and Setters
        public Unit unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
        public int? LectureAttendance
        {
            get { return _lectureAttendance; }
            set
            {
                _lectureAttendance = value;
                OnPropertyChanged("LectureAttendance");
            }
        }
        public int? PracticalAttendance
        {
            get { return _practicalAttendance; }
            set
            {
                _practicalAttendance = value;
                OnPropertyChanged("PracticalAttendance");
            }
        }
        public string StaffFeedback
        {
            get { return _staffFeedback; }
            set { _staffFeedback = value; }
        }
        public string StudentFeedback
        {
            get { return _studentFeedback; }
            set { _studentFeedback = value; }
        }
        public Account account
        {
            get { return _account; }
            set { _account = value; }
        }
        public bool? AtRisk
        {
            get { return _atRisk; }
            set { _atRisk = value; }
        }
        #endregion

        public string IsAtRisk
        {
            get
            {
                if (_atRisk == null || _atRisk == false) return "No";
                else return "Yes";
            }
            private set { }
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
