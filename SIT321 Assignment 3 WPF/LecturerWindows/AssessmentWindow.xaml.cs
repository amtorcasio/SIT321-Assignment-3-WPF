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
using SARMS.Content;
using SARMS.Users;

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for AssessmentWindow.xaml
    /// </summary>
    public partial class AssessmentWindow : Window
    {
        private Unit _unit = null;
        private Window _from;
        private Lecturer _loggedIn;

        private AssessmentWindow(Lecturer loggedIn, Window from)
        {
            InitializeComponent();

            _loggedIn = loggedIn;

            _from = from;
            _from.IsEnabled = false;
        }

        public AssessmentWindow(Lecturer loggedIn, Window from, ref Unit unit)
            : this (loggedIn, from)
        {
            _unit = unit;

            btnSubmit.Content = "Create New Asessment";
            this.Title = "Create New Assessment";
            txtUnitName.Text = unit.Name;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.IsEnabled = true;
            _from.Focus();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string errorString = string.Empty;
            int totalMarks;
            double weight;
            if (!int.TryParse(txtTotalMarks.Text, out totalMarks))
            {
                errorString += "Invalid total marks value";
            }
            if (!double.TryParse(txtWeight.Text, out weight))
            {
                if (!string.IsNullOrEmpty(errorString)) errorString += Environment.NewLine;
                errorString += "Invalid weight value";
            }
            if (!string.IsNullOrEmpty(errorString))
            {
                errorString += Environment.NewLine + Environment.NewLine + "Do you wish to try again?";
                var result = MessageBox.Show(errorString, "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes);
                if (result == MessageBoxResult.No) this.Close();
            }

            var tempAss = new Assessment(txtAssessmentName.Text, totalMarks, weight, _unit);
            if (!_loggedIn.AddAssessment(_unit, tempAss))
            {
                var msgResult = MessageBox.Show("An error occured with editing the database, retry?", "Database Error", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (msgResult == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
            this.Close(); 
        }
    }
}
