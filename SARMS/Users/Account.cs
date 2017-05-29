using System.Data.SQLite;
using SARMS;

namespace SARMS.Users
{
    public class Account
    {
        //private fields
        private int _ID;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;

        //public fields
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        //removed; usertype Administrator does not directly participate in any unit.
        //public List<Unit> Units { get; set; }

        public void changePassword()
        {
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
