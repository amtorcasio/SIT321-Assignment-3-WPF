﻿using System;
using System.IO;
using System.Collections.Generic;
using SARMS.Users;
using SARMS.Content;
using System.Data.SQLite;
using System.Net.Mail;
using SARMS.Data;

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
        public static SQLiteConnection GetDatabaseSQLConnection()
        {
            return new SQLiteConnection(@"Data Source=" + GetPathData() + "Database.db;");
        }

        public static string GetPathData()
        {
            return Directory.GetParent(Directory.GetParent(Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).ToString()).ToString()) + "\\Data\\";
        }

        // getStudentData accessible by UserTypes Administrator, Lecturer and Student
        public static Tuple<List<StudentAssessment>, StudentUnit> GetStudentData(Student s, Unit u)
        {
            List<StudentAssessment> performance = s.Performance.FindAll(e => (e.Assessment.unit.ID == u.ID));
            StudentUnit attendance = s.Units.Find(e => (e.unit.ID == u.ID));
            return new Tuple<List<StudentAssessment>, StudentUnit>(performance, attendance);
        }
        
        // isStudentAtRisk accessible by UserTypes Administrator and Lecturer
        public static bool IsStudentAtRisk(Student s)
        {
            throw new NotImplementedException();
        }

        // alertStudentAtRisk called as a result of conditions triggered in isStudentAtRisk
        // email sent to any relevant users identified in isStudentAtRisk
        private static void AlertStudentAtRisk(List<Account> accounts, Student atRisk)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.google.com";
            foreach (Account a in accounts)
            {
                MailMessage message = new MailMessage("admin@sarms.edu.au", a.Email);
                message.Subject = "The student " + atRisk.LastName + " " + atRisk.FirstName + " has been identified at risk";
                message.Body = "Please check the feeback within the SARMS application";
                client.Send(message);
            }
        }

        /* 
        protected DataContext GetDatabaseDataContext()
        {
            return new DataContext(@"Data Source=(LocalDB)\v12.0; AttachDbFilename='" + PATH_DATA + "Database.mdf'; Integrated Security=True");
        }*/

        /* implement function to query the SQL database
         * we might want to do this for the purposes of login and forgot password
         */
    }
}
