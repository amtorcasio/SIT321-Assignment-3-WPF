﻿namespace SIT321_Assignment_3_WPF.Users
{
    class Lecturer : User
    {
        // constructor
        public Lecturer(string fname, string lname, string email, string password)
        {
            Firstname = fname;
            Lastname = lname;
            Email = email;
            Password = password;
        }

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

        public void addFeedback(Student s, Unit u)
        { }

        public void getFeedback(Student s, Unit u)
        { }

        public void getStudentData(Student s, Unit u)
        { }

        public void generateReport(Student s, Unit u)
        { }
    }
}
