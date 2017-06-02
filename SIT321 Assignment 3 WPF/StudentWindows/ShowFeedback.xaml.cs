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

namespace SIT321_Assignment_3_WPF.StudentWindows
{
    public class FeedbackItem
    {
        public string   Name { get; set; }
        public DateTime Timestamp { get; set; }
        public string   Comment { get; set; }
        
        public FeedbackItem(string name, DateTime tstamp, string comment)
        {
            Name        = name;
            Timestamp   = tstamp;
            Comment     = comment;
        }
    }
    /// <summary>
    /// Interaction logic for ShowFeedback.xaml
    /// </summary>
    public partial class ShowFeedback : Window
    {
        public ShowFeedback()
        {
            InitializeComponent();

            List<FeedbackItem> lfbi = new List<FeedbackItem>();
            lfbi.Add(new FeedbackItem("Damian Weiss", DateTime.Now, "Welcome to my unit!"));
            lfbi.Add(new FeedbackItem("Lucy Mary", DateTime.Now, "Hi Damian, hope to have a great trimester with you!"));

            lsbFeedbackList.ItemsSource = lfbi;
        }

        private void btnGiveFeedback_Click(object sender, RoutedEventArgs e)
        {
            var giveFeedbackWindow = new GiveFeedback();
            giveFeedbackWindow.Show();
            giveFeedbackWindow.Focus();
        }
    }
}
