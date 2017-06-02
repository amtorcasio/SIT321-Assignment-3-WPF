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

namespace SIT321_Assignment_3_WPF.StudentWindows
{
    public class FeedbackItem
    {
        public DateTime Timestamp { get; set; }
        public string   Comment { get; set; }
        
        public FeedbackItem(DateTime tstamp, string comment)
        {
            Timestamp = tstamp;
            Comment     = comment;
        }
    }
    /// <summary>
    /// Interaction logic for ShowFeedback.xaml
    /// </summary>
    public partial class ShowFeedback : Window
    {
        public List<FeedbackItem> allFeedback;

        public Account loggedInAccount;
        public int selectedUnit;

        public ShowFeedback(Account a, int unit)
        {
            InitializeComponent();

            loggedInAccount = a;
            selectedUnit = unit;

            OutputAllFeedback();

        }

        private void OutputAllFeedback()
        {
            allFeedback = new List<FeedbackItem>();
            using (var conn = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand c = null;
                SQLiteDataReader r = null;

                try
                {
                    conn.Open();

                    c = conn.CreateCommand();
                    c.CommandText = "SELECT StaffFeedback, StudentFeedback FROM UserUnits WHERE UserID = @user AND UnitID = @id";
                    c.Parameters.AddWithValue("@user", loggedInAccount.ID);
                    c.Parameters.AddWithValue("@id", selectedUnit);
                    
                    r = c.ExecuteReader();

                    if (r.HasRows && r.Read())
                        if (r[0].ToString() != "")
                        {
                            string[] staffFeedback = r[0].ToString().Split('\n');
                            string[] studentFeedback = r[1].ToString().Split('\n');

                            foreach (string s in staffFeedback)
                                allFeedback.Add(new FeedbackItem(DateTime.Parse(s.Substring(s.IndexOf('<') + 1)), s.Remove(s.IndexOf('<') + 1)));

                            foreach (string s in studentFeedback)
                            {
                                DateTime d = DateTime.Parse(s.Substring(s.IndexOf('<') + 1));
                                allFeedback.Insert(0, new FeedbackItem(d, s.Remove(s.IndexOf('<') + 1)));
                                for (int i = 0; i < allFeedback.Count - 1; i++)
                                {
                                    if (allFeedback[i].Timestamp > allFeedback[i + 1].Timestamp)
                                    {
                                        var temp = allFeedback[i];
                                        allFeedback[i] = allFeedback[i + 1];
                                        allFeedback[i + 1] = temp;
                                    }
                                    else
                                        break;
                                }

                            }
                        }
                }
                finally
                {
                    if (c != null) c.Dispose();
                    if (r != null) r.Close();
                }
            }

            if (allFeedback.Count == 0)
                lsbFeedbackList.Items.Add(new ListBoxItem() { Content = "No feedback available. Check again later!", Padding = new Thickness(10,5,5,5) });
            else
                lsbFeedbackList.ItemsSource = allFeedback;
        }

        private void btnGiveFeedback_Click(object sender, RoutedEventArgs e)
        {
            var giveFeedbackWindow = new GiveFeedback();
            giveFeedbackWindow.Show();
            giveFeedbackWindow.Focus();
        }

    }
}
