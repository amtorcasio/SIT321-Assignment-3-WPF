using SARMS.Content;

namespace SARMS.Data
{
    public class StudentAssessment
    {
        private Assessment _assessment;
        private decimal _mark;

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
