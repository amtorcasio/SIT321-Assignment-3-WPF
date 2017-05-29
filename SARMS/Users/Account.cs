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
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        //removed; usertype Administrator does not directly participate in any unit.
        //public List<Unit> Units { get; set; }

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

        public void forgotPassword()
        {
        }

        public static string PATH_DATA = Utilities.GetPathData();

        protected SQLiteConnection GetDatabaseSQLConnection()
        {
            return new SQLiteConnection(@"Data Source="+ PATH_DATA + "Database.db;");
        }

        /*
        protected DataContext GetDatabaseDataContext()
        {
            return new DataContext(@"Data Source=(LocalDB)\v12.0; AttachDbFilename='" + PATH_DATA + "Database.mdf'; Integrated Security=True");
        }*/

    }
}
