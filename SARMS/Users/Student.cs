using System;
using System.Collections.Generic;

namespace SARMS.Users
{
    class Student : Account
    {
        // constructor
        public Student(string fname, string lname, string email, string password)
        {
            Firstname = fname;
            Lastname = lname;
            Email = email;
            Password = password;

            Units = new Dictionary<Unit, Tuple<decimal, int>>();
        }

        public bool AtRisk { get; set; }
        //public decimal Performance { get; set; }
        //public int Attendance { get; set; }
        public Dictionary<Unit, Tuple<decimal, int>> Units; // Student.Units hides inherited member Account.Units

        /* moved to utilities.cs
        public void addFeedback(Student s, Unit u)
        { }

        public void getFeedback(Student s, Unit u)
        { }

        public void getStudentData(Student s, Unit u)
        { }
        */
    }
}
