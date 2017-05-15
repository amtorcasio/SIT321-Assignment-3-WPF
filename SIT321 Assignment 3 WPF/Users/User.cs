using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Linq;

namespace SIT321_Assignment_3_WPF.Users
{
    class User
    {
        public int ID;
        public string Firstname;
        public string Lastname;
        public string Email;
        private string Password;
        public List<Unit> Units;

        public void changePassword()
        { }

        public void forgotPassword()
        { }

        public string PATH_DATA = System.AppDomain.CurrentDomain.BaseDirectory + "\\Data\\";
        private SqlConnection GetDatabaseSQLConnection()
        {
            return new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" + PATH_DATA + "Database.mdf';Integrated Security=True");
        }

        private DataContext GetDatabaseDataContext()
        {
            return new DataContext(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename='" + PATH_DATA + "Database.mdf'; Integrated Security=True");
        }
    }
}
