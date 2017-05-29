using System.Collections.Generic;
using SARMS.Content;

namespace SARMS.Users
{
    public class Lecturer : Account
    {
        public List<Unit> Units; // Lecturer.Units hides inherited member Acccount.Units

        // constructor
        public Lecturer(int id, string firstName, string lastName, string email, string password) :
            base (id, firstName, lastName, email, password)
        { }

        public void addAssessment(Unit u, Assessment a)
        { }

        public void removeAssessment(Unit u, Assessment a)
        { }

        public void addStudentPerformance(Student s, Unit u, Assessment a, int Mark)
        { }

        public void editStudentPerformance(Student s, Unit u, Assessment a, int Mark)
        { }

        public void addStudentAttendance(Student s, Unit u, bool lec, bool prac)
        { }

        public void editStudentAttendance(Student s, Unit u, int lec, int prac)
        { }

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
