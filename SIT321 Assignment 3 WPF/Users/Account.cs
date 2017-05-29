using System.Data.SQLite;
using System.IO;

using SIT321_Assignment_3_WPF.Content;

namespace SIT321_Assignment_3_WPF.Users
{
    class Account
    {
        
        private int _ID
        public string ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        protected string Password { get; set; }
        
        //removed; usertype Administrator does not directly participate in any unit.
        //public List<Unit> Units { get; set; }

        public void changePassword()
        {
        }

        public void forgotPassword()
        {
        }

        public static string PATH_DATA = Directory.GetParent(Directory.GetParent(Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).ToString()).ToString()) + "\\Data\\";
        //public static string PATH_DATA = System.AppDomain.CurrentDomain.BaseDirectory + "Data\\";
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
