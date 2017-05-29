using System;
using System.Data.SQLite;

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

        //Public Methods
        public bool changePassword(string password)
        {
            var conn = GetDatabaseSQLConnection();

            try
            {

            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        public static bool forgotPassword()
        {
            return false;
        }

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
