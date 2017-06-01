﻿using System;
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

namespace SIT321_Assignment_3_WPF.MainWindows
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

        private void btnEnrol_Click(object sender, RoutedEventArgs e)
        {
            if(txtEnrolUnitCode.Text.Trim().Count() == 6)
            {
                // prepare unitcode
                string unitcode = txtEnrolUnitCode.Text.Trim().ToUpper();

                // get unit
                Unit enrol = LoggedInAccount.GetLatestUnit(unitcode);
            }
        }
    }
}
