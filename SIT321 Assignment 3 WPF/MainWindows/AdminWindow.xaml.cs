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
using System.Data.SQLite;

using SARMS;
using SARMS.Users;
using SARMS.Content;

namespace SIT321_Assignment_3_WPF
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public Administrator LoggedInAccount { get; private set; }

        private List<string> listedUsers;
        private List<string> listedUnits;
        EventHandler re_populate_lists;     // repopulate list with event handler

        public AdminWindow(Account lAccount)
        {
            LoggedInAccount = lAccount as Administrator;
            InitializeComponent();
            PopulateList();
            re_populate_lists = new EventHandler(PopulateList);
        }

        private void PopulateList(object sender, EventArgs e)
        {
            PopulateList();
            btnEditUser.IsEnabled = false;
            btnEditUnit.IsEnabled = false;
            btnAddUser.IsEnabled = true;
            btnAddUnit.IsEnabled = true;
        }

        private void PopulateList()
        {
            // clear listboxes if previously populated
            listedUsers = new List<string>();
            listedUnits = new List<string>();
            listUnits.Items.Clear();
            listUsers.Items.Clear();


            // Get database connection to populate listboxes
            using (var conn = Utilities.GetDatabaseSQLConnection())
            {
                try
                {
                    conn.Open();

                    SQLiteCommand c = conn.CreateCommand();
                    c.CommandText = "SELECT * FROM User";
                    using (SQLiteDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                listedUsers.Add(r[0].ToString());
                                ListBoxItem lbi = new ListBoxItem();
                                lbi.Content = String.Format("{0}, {1}", r[2], r[1]);
                                lbi.FontSize = 14;
                                lbi.Padding = new Thickness(5, 5, 5, 5);

                                switch ((UserType)Convert.ToInt32(r[3]))
                                {
                                    case UserType.Administrator:
                                        lbi.Background = System.Windows.Media.Brushes.OrangeRed;
                                        break;
                                    case UserType.Lecturer:
                                        lbi.Background = System.Windows.Media.Brushes.Aqua;
                                        break;
                                    case UserType.Student:
                                        lbi.Background = System.Windows.Media.Brushes.LightGoldenrodYellow;
                                        break;
                                    default:
                                        return;
                                }

                                listUsers.Items.Add(lbi);
                            }
                        }
                    }

                    c = conn.CreateCommand();
                    c.CommandText = "SELECT * FROM Unit";
                    using (SQLiteDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            int count = 0;
                            while (r.Read())
                            {
                                listedUnits.Add(r[0].ToString());
                                ListBoxItem lbi = new ListBoxItem();
                                lbi.Content = r[2].ToString()+": "+r[1].ToString();
                                lbi.FontSize = 14;
                                lbi.Padding = new Thickness(5, 5, 5, 5);

                                switch(count % 2)
                                {
                                    case 0:
                                        lbi.Background = System.Windows.Media.Brushes.LightGray;
                                        break;
                                    case 1:
                                        lbi.Background = System.Windows.Media.Brushes.SlateGray;
                                        break;
                                    default:
                                        return;
                                }

                                listUnits.Items.Add(lbi);
                                count++;
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


            // group both listboxes under one event handler
            listUnits.SelectionChanged += new SelectionChangedEventHandler(ListItem_Clicked);
            listUsers.SelectionChanged += new SelectionChangedEventHandler(ListItem_Clicked);
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            btnAddUser.IsEnabled = false;
            btnAddUnit.IsEnabled = false;
            var adduserWindow = new AdminWindows.SetUpAccount(LoggedInAccount);
            adduserWindow.Show();
            adduserWindow.Focus();

            adduserWindow.Closed += re_populate_lists;
        }

        private void txtDBQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DBFilterUnits.IsChecked == false && DBFilterUsers.IsChecked == false)
                {
                    // do we want to filter both tables at the same time?
                }
                else if (DBFilterUnits.IsChecked == true)
                {
                    MessageBox.Show("DB queried with only Units filtered");
                }
                else if (DBFilterUnits.IsChecked == false)
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

        private void ListItem_Clicked(object sender, RoutedEventArgs e)
        {
            if ((sender as ListBox).Name == listUsers.Name)
            {
                btnEditUser.IsEnabled = true;
                btnEditUnit.IsEnabled = false;
            }
            else
            {
                btnEditUser.IsEnabled = false;
                btnEditUnit.IsEnabled = true;
            }
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            btnAddUnit.IsEnabled = false;
            btnAddUser.IsEnabled = false;
            btnEditUser.IsEnabled = false;
            Account SelectedUser;
            using (var conn = Utilities.GetDatabaseSQLConnection())
            {
                try
                {
                    conn.Open();

                    using (SQLiteCommand c = conn.CreateCommand())
                    {
                        c.CommandText = "SELECT * FROM User WHERE Id = @id";
                        c.Parameters.AddWithValue("@id", listedUsers[listUsers.SelectedIndex]);

                        using (SQLiteDataReader r = c.ExecuteReader())
                        {
                            r.Read();

                            switch ((UserType)Convert.ToInt32(r[3]))
                            {
                                case UserType.Administrator:
                                    SelectedUser = new Administrator(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[6].ToString(), r[5].ToString());
                                    break;
                                case UserType.Lecturer:
                                    SelectedUser = new Lecturer(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[6].ToString(), r[5].ToString());
                                    break;
                                case UserType.Student:
                                    SelectedUser = new Student(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[6].ToString(), r[5].ToString());
                                    break;
                                default:
                                    return;
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
            }

            var editUserWindow = new AdminWindows.EditAccount(LoggedInAccount, SelectedUser);
            editUserWindow.Show();
            editUserWindow.Focus();
            editUserWindow.Closed += re_populate_lists;
        }

        private void btnAddUnit_Click(object sender, RoutedEventArgs e)
        {
            btnAddUnit.IsEnabled = false;
            btnAddUser.IsEnabled = false;
            var addunitwindow = new AdminWindows.AddUnit(LoggedInAccount);
            addunitwindow.Show();
            addunitwindow.Focus();

            addunitwindow.Closed += re_populate_lists;
        }

        private void btnEditUnit_Click(object sender, RoutedEventArgs e)
        {
            btnAddUnit.IsEnabled = false;
            btnAddUser.IsEnabled = false;
            btnEditUnit.IsEnabled = false;
            Unit SelectedUnit;

            SelectedUnit = LoggedInAccount.GetUnit( long.Parse(listedUnits[listUnits.SelectedIndex]) );

            var editUnitWindow = new AdminWindows.EditUnit(LoggedInAccount, SelectedUnit);
            editUnitWindow.Show();
            editUnitWindow.Focus();
            editUnitWindow.Closed += re_populate_lists;
        }
    }
}
