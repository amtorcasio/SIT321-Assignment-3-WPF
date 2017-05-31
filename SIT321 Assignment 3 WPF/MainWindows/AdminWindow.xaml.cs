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

namespace SIT321_Assignment_3_WPF
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public Administrator LoggedInAccount { get; private set; }

        public AdminWindow(Account lAccount)
        {
            LoggedInAccount = lAccount as Administrator;
            InitializeComponent();
            
            gridUserDetails.Children.Add(new ShowUserDetails(lAccount));
            PopulateList();
        }

        private void PopulateList(object sender, EventArgs e)
        {
            PopulateList();
        }

        private void PopulateList()
        {
            var conn = Utilities.GetDatabaseSQLConnection();

            try
            {
                conn.Open();

                System.Data.SQLite.SQLiteCommand c = conn.CreateCommand();
                c.CommandText = "SELECT * FROM User";
                System.Data.SQLite.SQLiteDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Content = String.Format("{0}, {1}", r[2], r[1]);
                        lbi.FontSize = 14;
                        lbi.Padding = new Thickness(5,5,5,5);

                        listUsers.Items.Add(lbi);
                    }
                }

                c = conn.CreateCommand();
                c.CommandText = "SELECT * FROM Unit";
                r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Content = r[1];
                        lbi.FontSize = 14;
                        lbi.Padding = new Thickness(5, 5, 5, 5);

                        listUnits.Items.Add(lbi);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var adduserWindow = new AdminWindows.SetUpAccount(LoggedInAccount);
            adduserWindow.Show();
            adduserWindow.Focus();

            adduserWindow.Closed += new EventHandler(PopulateList);
        }

        private void txtDBQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DBFilterUnits.IsChecked == false && DBFilterUsers.IsChecked == false)
                {
                    // do we want to filter both tables at the same time?
                }
                else if (DBFilterUnits.IsChecked == true && DBFilterUsers.IsChecked == false)
                {
                    MessageBox.Show("DB queried with only Units filtered");
                }
                else if (DBFilterUnits.IsChecked == false && DBFilterUsers.IsChecked == true)
                {
                    MessageBox.Show("DB queried with only Users filtered");
                }

            }
        }

        private void DBFilterUsers_Click(object sender, RoutedEventArgs e)
        {
            txtDBQuery.Focus();
        }

        private void DBFilterUnits_Click(object sender, RoutedEventArgs e)
        {
            txtDBQuery.Focus();
        }
    }
}
