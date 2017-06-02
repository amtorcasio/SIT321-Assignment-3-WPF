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

            lsvAssessments.ItemsSource = _unit.Assessments;
            tboUnit.Text = unit.Code + ": " + unit.Name;
            CheckVisibility();
        }

        private void CheckVisibility()
        {
            if (_unit.Assessments.Count > 0)
            {
                lsvAssessments.Visibility = Visibility.Visible;
                tboNoAssessments.Visibility = Visibility.Collapsed;
                btnAddAssignment.Visibility = Visibility.Visible;
                btnRemoveAssessment.Visibility = Visibility.Visible;
            }
            else
            {
                lsvAssessments.Visibility = Visibility.Hidden;
                tboNoAssessments.Visibility = Visibility.Visible;
                btnAddAssignment.Visibility = Visibility.Collapsed;
                btnRemoveAssessment.Visibility = Visibility.Collapsed;
            }
        }

        private void hypCreateAssessment_Click(object sender, RoutedEventArgs e)
        {
            btnAddAssignment_Click(sender, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.Show();
            _from.Focus();
        }

        private void btnAddAssignment_Click(object sender, RoutedEventArgs e)
        {
            var createWin = new AssessmentWindow(_loggedIn, this, ref _unit);
            createWin.Show();
            createWin.Focus();
        }

        private void btnRemoveAssessment_Click(object sender, RoutedEventArgs e)
        {
            if (lsvAssessments.Items.Count == -1)
            {
                lsvAssessments.SelectedIndex = 0;
            }
            if (lsvAssessments.SelectedIndex == -1)
            {
                MessageBox.Show("You must select an assignment in order to remove it", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            var assessment = lsvAssessments.SelectedItem as Assessment;
            var msgResult = MessageBox.Show("Are you sure you want to remove '" + assessment.Name + "'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (msgResult == MessageBoxResult.Yes)
            {
                _loggedIn.RemoveAssessment(_unit, lsvAssessments.SelectedItem as Assessment);
                CheckVisibility();
            }
        }
    }
}
