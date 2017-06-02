using SARMS.Content;
using SARMS.Users;

namespace SARMS.Data
{
    public class StudentAssessment
    {
        private Account _account;
        private Assessment _assessment;
        private double _mark;

        public Account account
        {
            get { return _account; }
            set { _account = value; }
        }
        public Assessment Assessment
        {
            get { return _assessment; }
            set { _assessment = value; }
        }
        public double Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }
    }
}
