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
    /// <summary>
    /// Interaction logic for GiveFeedback.xaml
    /// </summary>
    public partial class GiveFeedback : Window
    {
        public GiveFeedback()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnPostComment_Click(object sender, RoutedEventArgs e)
        {
            // do something with txtComment.Text
        }
    }
}
