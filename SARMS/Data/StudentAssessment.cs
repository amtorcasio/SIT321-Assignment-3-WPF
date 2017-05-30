using SARMS.Content;
using SARMS.Users;

namespace SARMS.Data
{
    public class StudentAssessment
    {
        private Account _account;
        private Assessment _assessment;
        private decimal _mark;

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
        public decimal Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }
    }
}
