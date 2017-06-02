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
using SARMS.Data;

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for EditPerformance.xaml
    /// </summary>
    public partial class EditPerformance : Window
    {
        private Lecturer _loggedIn;
        private Window _from;
        private StudentAssessment _performance;
        private Student _student;

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public EditPerformance(Lecturer loggedIn, Window from, ref StudentAssessment performance)
        {
            InitializeComponent();
            _loggedIn = loggedIn;
            _from = from;
            _from.IsEnabled = false;
            this.Focus();

            _performance = performance;
            txtMark.Text = _performance.Mark.ToString();
            txtTotalMark.Text = _performance.Assessment.ToString();

            lblStudent.Content += " " + _performance.account.LastName + " " + _performance.account.FirstName;
            lblUnit.Content += " " + _performance.Assessment.unit.Code + " " + _performance.Assessment.unit.Name;
            lblAssessment.Content += " " + _performance.Assessment.Name + " ";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            double mark;
            if (!double.TryParse(txtMark.Text, out mark))
            {
                var msgResult = MessageBox.Show("Invalid mark value" + Environment.NewLine + Environment.NewLine + "Do you wish to try again?", 
                    "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes);
                if (msgResult == MessageBoxResult.No)
                {
                    this.Close();
                }
                return;
            }
            if (!_loggedIn.EditStudentPerformance(_student, _performance.Assessment, mark))
            {
                var msgResult = MessageBox.Show("An error occured with editing the database, retry?", "Database Error", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (msgResult == MessageBoxResult.No)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.IsEnabled = true;
        }
    }
}
