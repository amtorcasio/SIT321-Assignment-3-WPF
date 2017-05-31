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

namespace SIT321_Assignment_3_WPF.MainWindows
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public Student LoggedInAccount { get; private set; }

        public StudentWindow(Account lAccount)
        {
            LoggedInAccount = lAccount as Student;
            InitializeComponent();

            
        }

        private void PopulateList(object sender, EventArgs e)
        {
            PopulateList();
        }

        private void PopulateList()
        {
            // clear list if previously populated
            lsbUnits.Items.Clear();

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
                        lbi.Padding = new Thickness(5, 5, 5, 5);

                        lsbUnits.Items.Add(lbi);
                    }
                }

                lsbUnits.SelectionChanged += new SelectionChangedEventHandler(ListItem_Clicked);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

            private void ListItem_Clicked(object sender, RoutedEventArgs e)
        {
            if ((sender as ListBox).Name == lsbUnits.Name)
                btnShowReport.IsEnabled = true;
        }
    }
}
