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
using SARMS.Data;
using System.Data.SQLite;

namespace SIT321_Assignment_3_WPF.MainWindows
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public Student LoggedInAccount { get; private set; }

        private List<string> listedUnits;

        public StudentWindow(Student student)
        {
            LoggedInAccount = student;
            InitializeComponent();
            //lsbUnits.ItemsSource = student.Units;

            // clear listboxes if previously populated
            listedUnits = new List<string>();
            lsbUnits.Items.Clear();



            // Get database connection to populate listboxes
            using (var conn = Utilities.GetDatabaseSQLConnection())
            {
                try
                {
                    conn.Open();

                    SQLiteCommand c = conn.CreateCommand();
                    
                    c.CommandText = "SELECT * FROM Unit INNER JOIN UserUnits ON UserUnits.UnitID = Unit.Id WHERE UserUnits.UserId = @id";
                    c.Parameters.AddWithValue("@id", LoggedInAccount.ID);
                    using (SQLiteDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            int count = 0;
                            while (r.Read())
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

                                lsbUnits.Items.Add(lbi);
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
    }

        
        private void btnShowReport_Click(object sender, RoutedEventArgs e)
        {
            btnShowReport.IsEnabled = false;
            var readFeedbackWindow = new StudentWindows.ShowFeedback();
            readFeedbackWindow.Show();
            readFeedbackWindow.Focus();
            lsbUnits.UnselectAll();
        }

        private void lsbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsbUnits.SelectedIndex != -1)
            {
                btnShowReport.IsEnabled = true;
            }
            else
            {
                btnShowReport.IsEnabled = false;
            }
        }
    }
}