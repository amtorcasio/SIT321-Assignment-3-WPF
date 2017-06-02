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

        public Student loggedInStudent;
        public int selectedUnit;

        public ShowFeedback(Student s, int unit)
        {
            InitializeComponent();

            loggedInStudent = s;
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
                    c.CommandText = "SELECT * FROM UserUnits WHERE UserID = @user AND UnitID = @id";
                    c.Parameters.AddWithValue("@user", loggedInStudent.ID);
                    c.Parameters.AddWithValue("@id", selectedUnit);
                    
                    r = c.ExecuteReader();
                    if (r.HasRows && r.Read())
                    {
                        string[] staffFeedback = r[4].ToString().Split('\n');
                        string[] studentFeedback = r[5].ToString().Split('\n');

                        string matchFullResult = "";
                        foreach (string s in staffFeedback)
                        {
                            foreach (Match m in Regex.Matches(s, @"[0-9A-Z\s].+?M"))
                                matchFullResult += m.Value;
                            
                            allFeedback.Add(new FeedbackItem(DateTime.Parse(matchFullResult), s.Substring(s.IndexOf('>') + 1)));
                        }

                        matchFullResult = "";
                        foreach (string s in studentFeedback)
                        {
                            foreach (Match m in Regex.Matches(s, @"[0-9A-Z\s].+?M"))
                                matchFullResult += m.Value;

                            DateTime d = DateTime.Parse(matchFullResult);
                            for (int i = 0; i < allFeedback.Count; i++)
                                if (d < allFeedback[i].Timestamp)
                                    allFeedback.Insert(i, new FeedbackItem(d, s.Substring(s.IndexOf('>') + 1)));
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
