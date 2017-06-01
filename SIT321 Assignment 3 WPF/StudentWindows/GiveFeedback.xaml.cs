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

namespace SIT321_Assignment_3_WPF.StudentWindows
{
    public class FeedbackItem
    {
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PublishedComment { get; set; }
        public List<Tuple<Account, DateTime, string>> FeedbackReplies;

        public FeedbackItem(string usr, DateTime tstamp, string comment)
        {
            UserName = usr;
            TimeStamp = tstamp;
            PublishedComment = comment;
            FeedbackReplies = new List<Tuple<Account, DateTime, string>>();
        }
    }

    /// <summary>
    /// Interaction logic for GiveFeedback.xaml
    /// </summary>
    public partial class GiveFeedback : Window
    {
        public GiveFeedback()
        {
            InitializeComponent();

            lsbFeedbackList.Items.Add(new FeedbackItem("John Smith", DateTime.Now, "Hello World"));
            lsbFeedbackList.Items.Add(new FeedbackItem("Amy Marilyn", DateTime.Now, "Hello John"));
            /*
            lsbFeedbackList.Items.Add(new FeedbackItem("Maggie Harold", DateTime.Now, "Hello Amy"));
            lsbFeedbackList.Items.Add(new FeedbackItem("Susan Ford", DateTime.Now, "Hello Maggie"));
            */
        }
    }
}
