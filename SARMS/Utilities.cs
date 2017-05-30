using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        
        // IsStudentAtRisk accessible by UserTypes Administrator and Lecturer
        public static bool IsStudentAtRisk(Student student)
        {
            student.AtRisk = false;
            List<Account> accounts = new List<Account>();
            //Check attendance
            foreach (StudentUnit su in student.Units)
            {
                if ((su.LectureAttendance < (su.unit.TotalLectures / 2)) || 
                    (su.PracticalAttendance < (su.unit.TotalPracticals / 2)))
                {
                    student.AtRisk = true;
                    accounts.AddRange(FindLecturers(su.unit));
                }
            }
            //Check performance
            foreach (StudentAssessment sa in student.Performance)
            {
                if ((sa.Mark / sa.Assessment.TotalMarks) < 50.0M)
                {
                    student.AtRisk = true;
                    accounts.AddRange(FindLecturers(sa.Assessment.unit));
                }
            }
            //Compute alerts
            if (student.AtRisk)
            {
                accounts.AddRange(FindAllAdmins());
                AlertStudentAtRisk(accounts, student);
            }
            return student.AtRisk;
        }

        //todo: implement
        private static List<Lecturer> FindLecturers(Unit u)
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            var connection = GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM User INNER JOIN UserUnits on User.Id = UserUnits.UserID WHERE UserUnits.UnitID = @unitID";
                command.Parameters.AddWithValue("@unitID", u.ID);

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lecturers.Add(new Lecturer(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString()));
                    }
                }
                return lecturers;
            }
            catch (Exception e)
            {
                throw new Exception("A databases error occurred while searching for Lecturers related to unit " + u.ID, e);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
        }
        //todo: implement
        private static List<Administrator> FindAllAdmins()
        {
            List<Administrator> admins = new List<Administrator>();
            var connection = GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM User WHERE Type = 0";

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        admins.Add(new Administrator(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString()));
                    }
                }
                return admins;
            }
            catch (Exception e)
            {
                throw new Exception("A databases error occurred while retrieving all Administrators", e);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
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
