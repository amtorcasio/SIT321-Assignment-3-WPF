using System;
using System.Collections.Generic;
using SARMS.Content;
using SARMS.Data;

namespace SARMS.Users
{
    public class Student : Account
    {
        // constructor
        public Student(string id, string firstName, string lastName, string email, string password):
            base (id, firstName, lastName, email, password)
        { }

        public bool AtRisk { get; set; }

        public List<StudentAssessment> Performance;
        public List<StudentUnit> Units;

        //public decimal Performance { get; set; }
        //public Dictionary<Unit, Tuple<decimal, int>> Units;

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
