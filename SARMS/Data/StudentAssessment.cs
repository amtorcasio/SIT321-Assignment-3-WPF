namespace SARMS.Data
{
    class StudentAssessment
    {
        private string _accountID;
        private int _assessmentID;
        private decimal _mark;

        public string AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public int AssessmentID
        {
            get { return _assessmentID; }
            set { _assessmentID = value; }
        }
        public decimal Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }
    }
}
