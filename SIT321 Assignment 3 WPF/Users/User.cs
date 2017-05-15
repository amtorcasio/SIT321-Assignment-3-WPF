using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Linq;

namespace SIT321_Assignment_3_WPF.Users
{
    class User
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        private string Password { get; set; }
        public List<Unit> Units { get; set; }

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
