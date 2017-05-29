namespace SARMS.Data
{
    public class StudentUnit
    {
        private string _accountID;
        private int _unitID;
        private int _lectureAttendance;
        private int _practicalAttendance;
        private string _staffFeedback;
        private string _studentFeedback;

        #region Getters and Setters
        public string AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public int UnitID
        {
            get { return _unitID; }
            set { _unitID = value; }
        }
        public int LectureAttendance
        {
            get { return _lectureAttendance; }
            set { _lectureAttendance = value; }
        }
        public int PracticalAttendance
        {
            get { return _practicalAttendance; }
            set { _lectureAttendance = value }
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
