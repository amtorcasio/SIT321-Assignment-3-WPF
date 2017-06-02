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
using System.Text.RegularExpressions;
using SARMS;
using SARMS.Users;
using SARMS.Content;
using SIT321_Assignment_3_WPF.LecturerWindows;

namespace SIT321_Assignment_3_WPF.MainWindows
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public Administrator LoggedInAccount { get; private set; }
        Lecturer gimmeprivs = new Lecturer("admin.sarms", "", "", "", "");

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
            btnEditUser.IsEnabled = btnEditUnit.IsEnabled = false;
            btnAddUser.IsEnabled = btnAddUnit.IsEnabled = true;
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
            btnEditUser.IsEnabled = false;
            btnAddUser.IsEnabled = false;
            btnAddUnit.IsEnabled = false;
            var adduserWindow = new AdminWindows.SetUpAccount(LoggedInAccount);
            adduserWindow.Show();
            adduserWindow.Focus();

            adduserWindow.Closed += re_populate_lists;
        }

        private void txtDBQuery_KeyDown(object sender, KeyEventArgs e)
        {
            /* to reset the list before a query is made, clear the textbox then press the Enter key
             * neither listboxes will reset when changing filters mid-search
             */
            if (e.Key == Key.Enter && txtDBQuery.Text != "")
            {
                if (DBFilterUnits.IsChecked == false && DBFilterUsers.IsChecked == false)
                {
                    MessageBox.Show("Select a filter");
                }
                else
                {
                    using (var conn = Utilities.GetDatabaseSQLConnection())
                    {
                        SQLiteCommand c = null;

                        try
                        {
                            conn.Open();

                            c = conn.CreateCommand();
                            if (DBFilterUnits.IsChecked == true)
                            {
                                c.CommandText = "SELECT * FROM Unit WHERE Name LIKE @word OR Code LIKE @word";
                                c.Parameters.AddWithValue("@word", "%" + txtDBQuery.Text + "%");

                                using (SQLiteDataReader r = c.ExecuteReader())
                                {
                                    if (r.HasRows)
                                    {
                                        listUnits.Items.Clear();
                                        listedUnits.Clear();

                                        for (int count = 0; r.Read(); count++)
                                        {
                                            listedUnits.Add(r[0].ToString());
                                            ListBoxItem lbi = new ListBoxItem();
                                            lbi.Content = r[2].ToString() + ": " + r[1].ToString();
                                            lbi.FontSize = 14;
                                            lbi.Padding = new Thickness(5, 5, 5, 5);

                                            switch (count % 2)
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
                                        }
                                    }
                                }
                            }
                            else
                            {
                                c.CommandText = "SELECT * FROM User WHERE FirstName LIKE @word OR LastName LIKE @word";
                                c.Parameters.AddWithValue("@word", "%" + txtDBQuery.Text + "%");

                                using (SQLiteDataReader r = c.ExecuteReader())
                                {
                                    if (r.HasRows)
                                    {
                                        listUsers.Items.Clear();
                                        listedUsers.Clear();

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
                            }
                        }
                        catch (Exception exc)
                        {
                            throw exc;
                        }
                        finally
                        {
                            if (c != null) c.Dispose();
                        }
                    }
                }
            }
            else if (e.Key == Key.Enter && txtDBQuery.Text == "")
                PopulateList();
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
            if (listUsers.SelectedIndex < 0)
            {
                return;
            }
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
            btnEditUnit.IsEnabled = false;
            btnAddUnit.IsEnabled = false;
            btnAddUser.IsEnabled = false;
            var addunitwindow = new AdminWindows.AddUnit(LoggedInAccount);
            addunitwindow.Show();
            addunitwindow.Focus();

            addunitwindow.Closed += re_populate_lists;
        }

        private void btnEditUnit_Click(object sender, RoutedEventArgs e)
        {
            if (listUnits.SelectedIndex >= 0)
            {
                btnAddUnit.IsEnabled = false;
                btnAddUser.IsEnabled = false;
                btnEditUnit.IsEnabled = false;
                Unit SelectedUnit;

                SelectedUnit = LoggedInAccount.GetUnit(long.Parse(listedUnits[listUnits.SelectedIndex]));

                var editUnitWindow = new AdminWindows.EditUnit(LoggedInAccount, SelectedUnit);
                editUnitWindow.Show();
                editUnitWindow.Focus();
                editUnitWindow.Closed += re_populate_lists;
            }
        }

        private void btnEnrol_Click(object sender, RoutedEventArgs e)
        {
            if(txtEnrolUnitCode.Text.Trim().Count() == 6)
            {
                if (listUsers.SelectedIndex >= 0)
                {
                    // prepare unitcode
                    string unitcode = txtEnrolUnitCode.Text.Trim().ToUpper();

                    // get unit
                    Unit enrol = LoggedInAccount.GetLatestUnit(unitcode);
                    if (enrol == null)
                    {
                        MessageBox.Show("Unit does not exist");
                        return;
                    }
                    else
                    {
                        if(LoggedInAccount.isAccountEnrolled(enrol.ID, listedUsers[listUsers.SelectedIndex]))
                        {
                            MessageBox.Show("Account already Enrolled to Unit");
                            return;
                        }
                    }

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
                                            MessageBox.Show("You cannot enrol a fellow administrator to a unit");
                                            r.Close();
                                            c.Dispose();
                                            conn.Close();
                                            return;
                                        case UserType.Lecturer:
                                            Lecturer templec = new Lecturer(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[6].ToString(), r[5].ToString());
                                            r.Close();
                                            c.Dispose();
                                            conn.Close();
                                            if(LoggedInAccount.AddLecturerUnit(templec, enrol))
                                            {
                                                MessageBox.Show("Lecturer " + templec.FirstName + " " + templec.LastName + " has been assigned to Unit");
                                            }
                                            return;
                                        case UserType.Student:
                                            Student tempstu = new Student(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[6].ToString(), r[5].ToString());
                                            r.Close();
                                            c.Dispose();
                                            conn.Close();
                                            if(LoggedInAccount.AddStudentUnit(tempstu, enrol))
                                            {
                                                MessageBox.Show("Student " + tempstu.FirstName + " " + tempstu.LastName + " has been enrolled to Unit");
                                                PopulateList();
                                            }
                                            return;
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
                }
                else
                {
                    MessageBox.Show("You must select an account to enrol to unit");
                    return;
                }
            }
            else
            {
                MessageBox.Show("UnitCode must be 6 character!");
                return;
            }
        }

        private void btnUnenrol_Click(object sender, RoutedEventArgs e)
        {
            // if unitcode is appropriate length
            if (txtUnenrolUnitCode.Text.Trim().Count() == 6)
            {
                // if user is selected
                if (listUsers.SelectedIndex >= 0)
                {
                    // prepare unitcode
                    string unitcode = txtUnenrolUnitCode.Text.Trim().ToUpper();

                    // get unit
                    Unit enrol = LoggedInAccount.GetLatestUnit(unitcode);
                    if (enrol == null)
                    {
                        MessageBox.Show("Unit does not exist");
                        return;
                    }
                    else
                    {
                        if (!LoggedInAccount.isAccountEnrolled(enrol.ID, listedUsers[listUsers.SelectedIndex]))
                        {
                            MessageBox.Show("Account is not Enrolled to Unit");
                            return;
                        }
                    }

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
                                            MessageBox.Show("Administrator cannot be passed into this command");
                                            r.Close();
                                            c.Dispose();
                                            conn.Close();
                                            return;
                                        case UserType.Lecturer:
                                            Lecturer templec = new Lecturer(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[6].ToString(), r[5].ToString());
                                            r.Close();
                                            c.Dispose();
                                            conn.Close();
                                            if (LoggedInAccount.RemoveLecturerUnit(templec, enrol))
                                            {
                                                MessageBox.Show("Lecturer " + templec.FirstName + " " + templec.LastName + " has been unassigned from Unit");
                                            }
                                            return;
                                        case UserType.Student:
                                            Student tempstu = new Student(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[6].ToString(), r[5].ToString());
                                            r.Close();
                                            c.Dispose();
                                            conn.Close();
                                            if (LoggedInAccount.RemoveStudentUnit(tempstu, enrol))
                                            {
                                                MessageBox.Show("Student " + tempstu.FirstName + " " + tempstu.LastName + " has been unenrolled from Unit");
                                                PopulateList();
                                            }
                                            return;
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
                }
                else
                {
                    MessageBox.Show("You must select an account to unenrol from unit");
                    return;
                }
            }
            else
            {
                MessageBox.Show("UnitCode must be 6 character!");
                return;
            }
        }

        private void btnViewStudentData_Click(object sender, RoutedEventArgs e)
        {
            if (listUnits.SelectedIndex >= 0)
            {
                Unit SelectedUnit;
                SelectedUnit = LoggedInAccount.GetUnit(long.Parse(listedUnits[listUnits.SelectedIndex]));

                var listWindow = new StudentList(gimmeprivs, SelectedUnit, this);

                listWindow.Show();
                listWindow.Focus();
                this.Hide();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PopulateList();
            DBFilterUnits.IsChecked = false;
            DBFilterUsers.IsChecked = false;
            txtDBQuery.Text = string.Empty;
            txtEnrolUnitCode.Text = string.Empty;
            txtUnenrolUnitCode.Text = string.Empty;
            MessageBox.Show("Window has been Refreshed");
        }

        private void listUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listUsers.SelectedIndex >= 0)
            {
                string daid = listedUsers[listUsers.SelectedIndex];

                if ( LoggedInAccount.GetType(daid) == 0 )
                {
                    MessageBox.Show("Cannot Select an Administrator!");
                    return;
                }

                Account tempacc;
                listUnits.Items.Clear();

                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    SQLiteDataReader r = null;

                    try
                    {
                        connection.Open();
                        command = connection.CreateCommand();

                        command.CommandText = "SELECT * FROM User WHERE Id = @userid";
                        command.Parameters.AddWithValue("@userid", listedUsers[listUsers.SelectedIndex]);
                        r = command.ExecuteReader();

                        if (r.HasRows)
                        {
                            r.Read();
                            tempacc = new Account(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[7].ToString(), r[6].ToString());
                        }
                        else
                            return;
                    }
                    finally
                    {
                        if (r != null) r.Close();
                        if (command != null) command.Dispose();
                        if (connection != null) connection.Close();
                    }
                }

                List<Unit> resultunits = LoggedInAccount.GetUnitsbyAccount(tempacc);

                if(resultunits == null)
                {
                    listUnits.Items.Clear();
                    listedUnits = new List<string>();
                    return;
                }

                listedUnits = new List<string>();

                int count = 0;
                foreach (Unit u in resultunits)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Content = string.Format("{0}: {1}", u.Code, u.Name);
                    lbi.FontSize = 14;
                    lbi.Padding = new Thickness(5, 5, 5, 5);

                    switch (count % 2)
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
                    listedUnits.Add(u.ID.ToString());
                    count++;
                }

            }
        }

        private void listUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listUnits.SelectedIndex >= 0)
            {
                Unit temppass = new Unit(long.Parse(listedUnits[listUnits.SelectedIndex]), null, null, 0, 0, 0, 0);
                List<Account> unitusers = LoggedInAccount.SearchAccountsByUnit(temppass);
                listUsers.Items.Clear();
                listedUsers = new List<string>();

                foreach (Account a in unitusers)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Content = string.Format("{0}, {1}", a.FirstName, a.LastName);
                    lbi.FontSize = 14;
                    lbi.Padding = new Thickness(5, 5, 5, 5);

                    switch ((UserType)Convert.ToInt32(LoggedInAccount.GetType(a.ID)))
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
                    listedUsers.Add(a.ID);
                }
            }
        }

        private void txtDBQuery_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
