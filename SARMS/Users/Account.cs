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
        public static bool Login(string email, string password)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT 1 FROM Users WHERE Id = @email AND Password = @password";
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);

                SQLiteDataReader reader = command.ExecuteReader();

                int count = 0;
                while (reader.Read())
                    count++;

                if (count == 1)
                    return true;
            }
            catch (Exception e)
            {
                throw new Exception("A databases error occurred while logging in", e);
            }
            return false;
        }

        public bool ChangePassword(string password)
        {
            var conn = Utilities.GetDatabaseSQLConnection();

            return false;
        }

        public static bool ForgotPassword()
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
