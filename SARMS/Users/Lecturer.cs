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

        public void addStudentAttendance(Student s, Unit u, bool lec, bool prac)
        {
            if (!s.Units.ContainsKey(u))
            {
                s.Units.Add(u, new Tuple<decimal, int>(1, 1));
            }
        }

        public void editStudentAttendance(Student s, Unit u, int lec, int prac)
        {
            var data = s.Units[u];
            s.Units[u] = new Tuple<decimal, int>(data.Item1, data.Item2 + prac);
        }

        public List<Account> viewSAR(Unit u)
        {
            List<Account> SAR = new List<Account>();
            try
            {

            }
            catch (Exception e)
            {
                throw e;
            }

            return new List<Account>();
        }

        /* moved to utilities.cs
        public void addFeedback(Student s, Unit u)
        { }

        public void getFeedback(Student s, Unit u)
        { }

        public void getStudentData(Student s, Unit u)
        { }

        public void generateReport(Student s, Unit u)
        { }
        */
    }
}
