using System;
using System.IO;
using System.Collections.Generic;
using SARMS.Users;
using SARMS.Content;
using System.Data.SQLite;
using System.Net.Mail;
using SARMS.Data;
using System.Linq;
using System.Collections.ObjectModel;

namespace SARMS
{
    public enum UserType
    {
        Administrator,
        Lecturer,
        Student
    }

    // a group of methods and variables that is shared throughout the entire project
    public static class Utilities
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
        public static Tuple<ObservableCollection<StudentAssessment>, StudentUnit> GetStudentData(Student s, Unit u)
        {
            ObservableCollection<StudentAssessment> performance = new ObservableCollection<StudentAssessment>(s.Performance.Where(e => (e.Assessment.unit.ID == u.ID)).ToList());
            StudentUnit attendance = s.Units.Find(e => (e.unit.ID == u.ID));
            return new Tuple<ObservableCollection<StudentAssessment>, StudentUnit>(performance, attendance);
        }
        
        // IsStudentAtRisk accessible by UserTypes Administrator and Lecturer
        public static bool IsStudentAtRisk(Student student)
        {
            bool atRisk = false;
            List<Account> accounts = new List<Account>();
            //Check attendance
            foreach (StudentUnit su in student.Units)
            {
                if ((su.LectureAttendance < (su.unit.TotalLectures / 2)) || 
                    (su.PracticalAttendance < (su.unit.TotalPracticals / 2)))
                {
                    su.AtRisk = true;
                    atRisk = true;
                    accounts.AddRange(FindLecturers(su.unit));
                }
            }
            //Check performance
            foreach (StudentAssessment sa in student.Performance)
            {
                if ((sa.Mark / sa.Assessment.TotalMarks) < 50.0d)
                {
                    var su = student.Units.Find(e => (e.unit.ID == sa.Assessment.unit.ID));
                    su.AtRisk = true;
                    atRisk = true;
                    accounts.AddRange(FindLecturers(sa.Assessment.unit));
                }
            }
            //Compute alerts
            if (atRisk)
            {
                accounts.AddRange(FindAllAdmins());
                AlertStudentAtRisk(accounts, student);
            }
            return atRisk;
        }

        private static List<Lecturer> FindLecturers(Unit u)
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            using (var connection = GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();

                    command = connection.CreateCommand();
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
                    if (command != null) command.Dispose();
                    if (reader != null) reader.Close();
                    if (connection != null) connection.Close();
                }
            }
        }

        private static List<Administrator> FindAllAdmins()
        {
            List<Administrator> admins = new List<Administrator>();
            using (var connection = GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();

                    command = connection.CreateCommand();
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
                    if (command != null) command.Dispose();
                    if (reader != null) reader.Close();
                    if (connection != null) connection.Close();
                }
            }
        }

        // alertStudentAtRisk called as a result of conditions triggered in isStudentAtRisk
        // email sent to any relevant users identified in isStudentAtRisk
        private static void AlertStudentAtRisk(List<Account> accounts, Student atRisk)
        {
            string subject = "The student " + atRisk.LastName + " " + atRisk.FirstName + " has been identified at risk";
            string body = "Please check the feeback within the SARMS application";
            List<string> recipients = new List<string>();
            foreach (Account a in accounts)
            {
                recipients.Add(a.Email);
            }
            SendMailMessagesFromAdmin(recipients, subject, body);
        }

        public static void SendMailMessageFromAdmin(string recipient, string subject, string body)
        {
            List<string> recipients = new List<string>() { recipient };
            SendMailMessagesFromAdmin(recipients, subject, body);
        }

        public static void SendMailMessagesFromAdmin(List<string> recipients, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Timeout = 10000;
            client.Host = "smtp.gmail.com";
            client.Credentials = new System.Net.NetworkCredential("sarms.edu@gmail.com", "Sit321sarms");
            foreach (string address in recipients)
            {
                MailMessage message = new MailMessage("sarms.edu@gmail.com", address);
                message.Subject = subject;
                message.Body = body;
                client.Send(message);
            }
        }

        public static string CommentTail()
        {
            return "<"+DateTime.Now.ToString()+"|";
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
