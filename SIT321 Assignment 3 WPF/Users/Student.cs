﻿using System;

namespace SIT321_Assignment_3_WPF.Users
{
    public class Student : Account
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
        //public decimal Performance { get; set; }
        //public int Attendance { get; set; }
        public System.Collections.Generic.Dictionary<Unit, Tuple<decimal, int>> Units;

        public void addFeedback(Student s, Unit u)
        { }

        public void getFeedback(Student s, Unit u)
        { }

        public void getStudentData(Student s, Unit u)
        { }
    }
}
