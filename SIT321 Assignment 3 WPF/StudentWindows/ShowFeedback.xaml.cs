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

        public Student loggedInStudent;
        public int selectedUnit;

        public ShowFeedback()
        {
            InitializeComponent();
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
                    c.CommandText = "SELECT * FROM UserUnits WHERE UserID = @user AND UnitID = @id";
                    c.Parameters.AddWithValue("@user", loggedInStudent);
                    c.Parameters.AddWithValue("@id", selectedUnit);
                    
                    r = c.ExecuteReader();
                    if (r.HasRows && r.Read())
                    {
                        string[] staffFeedback = r[4].ToString().Split('\n');
                        string[] studentFeedback = r[5].ToString().Split('\n');

                        foreach (string s in staffFeedback)
                        {
                            string[] comment_item = new string[2] { new System.Text.RegularExpressions.Regex(@"[^<>]").Match(s).ToString(), s.Substring(s.IndexOf('>') + 1) };
                            allFeedback.Add(new FeedbackItem(DateTime.Parse(comment_item[0]), comment_item[1]));
                        }

                        foreach (string s in studentFeedback)
                        {
                            string[] comment_item = new string[2] { new System.Text.RegularExpressions.Regex(@"[^<>]").Match(s).ToString(), s.Substring(s.IndexOf('>') + 1) };
                            for (int i = 0; i < allFeedback.Count; i++)
                            {
                                DateTime d = DateTime.Parse(comment_item[0]);
                                if (d < allFeedback[i].Timestamp)
                                    allFeedback.Insert(i, new FeedbackItem(d, comment_item[1]));
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
