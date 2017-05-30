using System;
using System.Data.SQLite;
using SARMS.Content;
using SARMS.Data;

namespace SARMS.Users
{
    public class Account
    {
        //private fields
        protected string _ID;
        protected string _firstName;
        protected string _lastName;
        protected string _email;
        protected string _password;

        //public fields
        public static string PATH_DATA = Utilities.GetPathData();
        #region Getters and Setters
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        #endregion

        //Constructors
        public Account(Administrator creator)
        { }
        protected Account(string id, string firstName, string lastName, string email, string password)
        {
            _ID = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
        }

        #region Public Methods
        public static Account Login(string email, string password)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM User WHERE email = @email AND Password = @password";
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    switch ((UserType)Convert.ToInt32(reader[3]))
                    {
                        case UserType.Administrator:
                            return new Administrator(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                        case UserType.Lecturer:
                            return new Lecturer(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                        case UserType.Student:
                            return new Student(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                        default:
                            return null;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("A databases error occurred while logging in", e);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
        }

        public bool ChangePassword(string password)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE User SET Password = @password WHERE Id = @id";
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@id", _ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public static bool ForgotPassword(string email)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Password FROM User WHERE Email = @email";
                command.Parameters.AddWithValue("@email", email);

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    Utilities.SendMailMessageFromAdmin(email, "Password Restore", reader[0].ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public bool AddFeedBack(Account by, Student student, Unit unit, string feedback)
        {
            if (feedback.Trim().Length == 0) return false;

            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                string feedBackType = (by is Administrator || by is Lecturer) ? "StaffFeedback" : "StudentFeedback";
                command.CommandText = "SELECT " + feedBackType + " FROM UserUnits WHERE UserID = @userID AND UnitID = @unitID";
                command.Parameters.AddWithValue("@userID", student.ID);
                command.Parameters.AddWithValue("@unitID", unit.ID);
                reader = command.ExecuteReader();

                string currentFeedback = "";
                if (reader.HasRows)
                {
                    reader.Read();
                    currentFeedback = reader[0].ToString();
                }

                if (currentFeedback.Length > 0) currentFeedback += "\n";

                command.CommandText = "UPDATE UserUnits SET " + feedBackType + " = @feedback WHERE UserID = @userID AND UnitID = @unitID";
                string newfeedback = currentFeedback + feedback;
                command.Parameters.AddWithValue("@feeback", newfeedback);

                if (command.ExecuteNonQuery() == 0)
                    return false;

                var studentUnit = student.Units.Find(e => (e.unit.ID == unit.ID));
                if (by is Administrator || by is Lecturer)
                {
                    studentUnit.StaffFeedback = newfeedback;
                }
                else
                {
                    studentUnit.StudentFeedback = newfeedback;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public StudentUnit GetFeedback(Student student, Unit unit)
        {
            return student.Units.Find(e => (e.unit.ID == unit.ID));
        }

        public void GetFeedback(Student student, Unit unit, out string staffFeeback, out string studentFeedback)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT StaffFeedback, StudentFeedback FROM UserUnits WHERE UserID = @userID AND UnitID = @unitID";
                command.Parameters.AddWithValue("@userID", student.ID);
                command.Parameters.AddWithValue("@unitID", unit.ID);
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    staffFeeback = reader[0].ToString();
                    studentFeedback = reader[0].ToString();
                }

                staffFeeback = null;
                studentFeedback = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }
        #endregion
    }
}
