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
using SARMS.Users;
using SARMS.Content;
using SARMS.Data;

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for AssessmentList.xaml
    /// </summary>
    public partial class AssessmentList : Window
    {
        private Window _from;
        private Lecturer _loggedIn;
        private Unit _unit;

        public AssessmentList(Lecturer loggedIn, Unit unit, Window from)
        {
            InitializeComponent();

            _loggedIn = loggedIn;
            _from = from;
            _unit = unit;

            lsvAssessments.ItemsSource = unit.Assessments;
            lsvAssessments.Visibility = Visibility.Visible;
            tboNoAssessments.Visibility = Visibility.Collapsed;
        }

        private void hypCreateAssessment_Click(object sender, RoutedEventArgs e)
        {
            var createWin = new AssessmentWindow(this, ref _unit);
            createWin.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.Show();
            _from.Focus();
        }
    }
}
