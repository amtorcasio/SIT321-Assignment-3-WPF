using System;

namespace SIT321_Assignment_3_WPF.Users
{
    class Student : User
    {
        // constructor
        public Student(string fname, string lname, string email, string password)
        {
            Firstname = fname;
            Lastname = lname;
            Email = email;
            Password = password;
        }

        public bool AtRisk { get; set; }
        public decimal Performance { get; set; }
        public int Attendance { get; set; }
    }
}
