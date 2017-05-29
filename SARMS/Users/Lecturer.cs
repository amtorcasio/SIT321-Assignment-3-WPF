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

        public void AddAssessment(Unit u, Assessment a)
        {
            u.Assessments.Add(a);
        }

        public void RemoveAssessment(Unit u, Assessment a)
        {
            // find index of assessment in list and override it
            int i = u.Assessments.IndexOf(a);
            while (i < u.Assessments.Count - 1)
                u.Assessments[i] = u.Assessments[i++];

            // remove the last assessment on the list because it has overriden the previous list item
            u.Assessments.RemoveAt(i);
        }

        public void AddStudentPerformance(Student s, Unit u, Assessment a, int Mark)
        { }

        public void EditStudentPerformance(Student s, Unit u, Assessment a, int Mark)
        { }

        // boolean if the student attended the lecturer and practical or did not attend
        public void addStudentAttendance(Student s, Unit u, bool lec, bool prac)
        {

        }

        // direct editing of values
        public void editStudentAttendance(Student s, Unit u, int lec, int prac)
        {

        }

        public List<Account> viewSAR(Unit u)
        {
            return new List<Account>();
        }
    }
}
