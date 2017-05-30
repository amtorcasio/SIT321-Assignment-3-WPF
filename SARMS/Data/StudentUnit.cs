using SARMS.Content;

namespace SARMS.Data
{
    public class StudentUnit
    {
        private Unit _unit;
        private int _lectureAttendance;
        private int _practicalAttendance;
        private string _staffFeedback;
        private string _studentFeedback;

        #region Getters and Setters
        public Unit unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
        public int LectureAttendance
        {
            get { return _lectureAttendance; }
            set { _lectureAttendance = value; }
        }
        public int PracticalAttendance
        {
            get { return _practicalAttendance; }
            set { _practicalAttendance = value; }
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
        #endregion
    }
}
