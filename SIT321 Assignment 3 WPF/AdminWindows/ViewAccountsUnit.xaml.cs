using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SARMS;
using SARMS.Users;
using SARMS.Content;
using System.Data.SQLite;

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for ViewAccountsUnit.xaml
    /// </summary>
    public partial class ViewAccountsUnit : Window
    {
        private Administrator Admin;
        private Unit Unitee;

        // list to store users before populating listbox
        private List<string> UniteeUsers;

        private List<Account> UniteeUserAccount;

        public ViewAccountsUnit(Administrator admin, Unit unitee)
        {
            InitializeComponent();

            Admin = admin;      // make class admin equal to passed administrator
            Unitee = unitee;

            // create new list of strings
            UniteeUsers = new List<string>();

            // Get database connection to get user ids
            using (var conn = Utilities.GetDatabaseSQLConnection())
            {
                // try get user ids from UserUnits
                try
                {
                    conn.Open();

                    SQLiteCommand c = conn.CreateCommand();
                    c.CommandText = "SELECT * FROM UserUnits WHERE UnitID = @unitid";
                    c.Parameters.AddWithValue("@unitid",unitee.ID);
                    using (SQLiteDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                UniteeUsers.Add(r[0].ToString());
                            }
                        }
                    }
                    c.Dispose();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            // list account
            UniteeUserAccount = Admin.SearchAccountsByUnit(Unitee);

            foreach(Account a in UniteeUserAccount)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = string.Format("{0}, {1}", a.FirstName, a.LastName);
                lbi.FontSize = 14;
                lbi.Padding = new Thickness(5, 5, 5, 5);

                lstbAccounts.Items.Add(lbi);
            }
        }
    }
}
