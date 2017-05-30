using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SARMS.Content;

namespace SARMS.Users
{
    public class Lecturer : Account
    {
        public List<Unit> Units; // Lecturer.Units hides inherited member Acccount.Units

        // constructor
        public Lecturer(string id, string firstName, string lastName, string email, string password) :
            base (id, firstName, lastName, email, password)
        { }

        public void AddAssessment(Unit unit, Assessment assessment)
        {
            unit.Assessments.Add(assessment);
        }

        public void RemoveAssessment(Unit unit, Assessment assessment)
        {
            // find index of assessment in list and override it
            int i = unit.Assessments.IndexOf(assessment);
            while (i < unit.Assessments.Count - 1)
                unit.Assessments[i] = unit.Assessments[i++];

            // remove the last assessment on the list because it has overriden the previous list item
            unit.Assessments.RemoveAt(i);
        }

        public void AddStudentPerformance(Student student, Unit unit, Assessment aassessment, int mark)
        { }

        public void EditStudentPerformance(Student student, Unit unit, Assessment assessment, int mark)
        { }

        // boolean if the student attended the lecturer and practical or did not attend
        public void AddStudentAttendance(Student student, Unit unit, bool didAttentLecture, bool didAttendPractical)
        {

        }

        // direct editing of values
        public void EditStudentAttendance(Student student, Unit unit, int numLectures, int numPracticals)
        {

        }

        public List<Account> viewSAR(Unit unit)
        {
            return new List<Account>();
        }
    }
}
