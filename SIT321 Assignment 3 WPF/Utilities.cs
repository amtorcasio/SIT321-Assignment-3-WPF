using System;
using System.Collections.Generic;
using SIT321_Assignment_3_WPF.Users;
using SIT321_Assignment_3_WPF.Content;

namespace SIT321_Assignment_3_WPF
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
        // getStudentData accessible by UserTypes Administrator, Lecturer and Student
        public static void getStudentData(Student s, Unit u)
        {

        }
        
        // addFeedback accessible by UserTypes Administrator, Lecturer and Student
        public static void addFeedback(Student s, Unit u)
        {

        }

        // getFeedback accessible by UserTypes Administrator, Lecturer and Student
        public static void getFeedback(Student s, Unit u)
        {

        }

        // generateReport accessible by UserTypes Administrator and Lecturer
        public static void generateReport(Student s, Unit u)
        {

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
