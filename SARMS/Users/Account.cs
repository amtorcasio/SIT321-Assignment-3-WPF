using System;
using System.Data.SQLite;
using SARMS.Content;

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
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
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
                            return null;
                        case UserType.Student:
                            return null;
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
            var conn = Utilities.GetDatabaseSQLConnection();

            return false;
        }

        public static bool ForgotPassword(string email)
        {
            return false;
        }

        public bool AddFeedBack(Account by, Student student, Unit unit, string feedback)
        {
            return false;
        }

        public string GetFeedback(Account from, Student student, Unit unit)
        {
            return "";
        }
        #endregion

        //removed; usertype Administrator does not directly participate in any unit.
        //public List<Unit> Units { get; set; }
    }
}
