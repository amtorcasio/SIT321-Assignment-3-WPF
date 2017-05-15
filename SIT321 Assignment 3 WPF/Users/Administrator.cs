using System;

namespace SIT321_Assignment_3_WPF.Users
{
    class Administrator : User
    {
        // constructor
        public Administrator(string fname, string lname, string email, string password)
        {
            Firstname = fname;
            Lastname = lname;
            Email = email;
            Password = password;
        }

        public void addUser(string fname, string lname, string email, string pass)
        { }

        public void suspendUser(User u)
        { }

        public void terminateUser(User u)
        { }

        public void editUser(User u, string fname, string lname, string email, string pass)
        { }

        public void removeUser(User u)
        { }

        public void addUnit(string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        { }

        public void editUnit(Unit u, string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        { }

        public void removeUnit(Unit u)
        { }

        public void addStudentUnit(Student s, Unit u)
        { }

        public void removeStudentUnit(Student s, Unit u)
        { }

        public void addLecturerUnit(Lecturer lec, Unit u)
        { }
        
        public void removeLecturerUnit(Lecturer lec, Unit u)
        { }

        public void addFeedback(Student s, Unit u)
        { }

        public void getFeedback(Student s, Unit u)
        { }

        public void generateReport(Student s, Unit u)
        { }
    }
}
