namespace SARMS.Content
{
    public class Assessment
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
    }
}
