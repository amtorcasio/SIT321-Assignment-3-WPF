using System;
using System.IO;
using System.Collections.Generic;
using SARMS.Users;
using SARMS.Content;

namespace SARMS
{
    public enum UserType
    {
        Administrator,
        Lecturer,
        Student
    }

    // a group of methods and variables that is shared throughout the entire project
    static class Utilities
    {
        public static string GetPathData()
        {
            return Directory.GetParent(Directory.GetParent(Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).ToString()).ToString()) + "\\Data\\";
        }

        // getStudentData accessible by UserTypes Administrator, Lecturer and Student
        public static string getStudentData(Student s, Unit u)
        {
            Tuple<decimal, int> data = null;
            s.Units.TryGetValue(u, out data);

            return "Performance: " + data.Item1 + "; Attendance: " + data.Item2 + " out of " + u.TotalPracticals;
        }
        
        // isStudentAtRisk accessible by UserTypes Administrator and Lecturer
        public static bool isStudentAtRisk(Student s)
        {
            throw new NotImplementedException();
        }

        // alertStudentAtRisk accessible by Lecturer
        public static void alertStudentAtRisk(List<Student> ls)
        {
            foreach (Student s in ls)
            {
                // implement alert
            }
        }

        /* implement function to query the SQL database
         * we might want to do this for the purposes of login and forgot password
         */
    }
}
