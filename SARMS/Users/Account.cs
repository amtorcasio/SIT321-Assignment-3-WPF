using System;
using System.Data.SQLite;
using SARMS.Content;

namespace SARMS.Users
{
    public class Account
    {
        //private fields
        protected int _ID;
        protected string _firstName;
        protected string _lastName;
        protected string _email;
        protected string _password;

        //public fields
        public static string PATH_DATA = Utilities.GetPathData();
        #region Getters and Setters
        public int ID
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
        protected Account(int id, string firstName, string lastName, string email, string password)
        {
            _ID = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
        }

        #region Public Methods
        public bool validateLogin(string id, string password)
        {
            var conn = GetDatabaseSQLConnection();

            try
            {
                conn.Open();

                SQLiteCommand c = conn.CreateCommand();
                c.CommandText = @"SELECT 1 FROM Users WHERE Id = " + id + " AND Password = " + password;

                SQLiteDataReader r = c.ExecuteReader();

                int count = 0;
                while (r.Read())
                    count++;

                if (count == 1)
                    return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        public bool ChangePassword(string password)
        {
            var conn = GetDatabaseSQLConnection();

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

        //Protected Methods
        protected SQLiteConnection GetDatabaseSQLConnection()
        {
            return new SQLiteConnection(@"Data Source="+ PATH_DATA + "Database.db;");
        }

        /* 
        protected DataContext GetDatabaseDataContext()
        {
            return new DataContext(@"Data Source=(LocalDB)\v12.0; AttachDbFilename='" + PATH_DATA + "Database.mdf'; Integrated Security=True");
        }*/

        //removed; usertype Administrator does not directly participate in any unit.
        //public List<Unit> Units { get; set; }
    }
}
