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

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for EditAttendanceWindow.xaml
    /// </summary>
    public partial class EditAttendanceWindow : Window
    {
        private Lecturer _loggedIn;
        private Window _from;
        private SARMS.Data.StudentUnit _unit;
        private Student _student;

        public EditAttendanceWindow(Lecturer loggedIn, Window from, ref Student student, long unitID)
        {
            InitializeComponent();
            _loggedIn = loggedIn;
            _from = from;
            _from.IsEnabled = false;
            this.Focus();
            _unit = student.Units.Find(e => (e.unit.ID == unitID));
            txtLectureAttendance.Text = _unit.LectureAttendance.ToString();
            txtPracticalAttendance.Text = _unit.PracticalAttendance.ToString();
            _student = student;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string errorString = string.Empty;
            int lectureAttendance;
            int practicalAttendance;
            if (!int.TryParse(txtLectureAttendance.Text, out lectureAttendance))
            {
                errorString += "Invalid lecture attendance value";
            }
            if (!int.TryParse(txtPracticalAttendance.Text, out practicalAttendance))
            {
                if (!string.IsNullOrEmpty(errorString)) errorString += Environment.NewLine;
                errorString += "Invalid practical attendance value";
            }
            if (!string.IsNullOrEmpty(errorString))
            {
                errorString += Environment.NewLine + Environment.NewLine + "Do you wish to try again?";
                var result = MessageBox.Show(errorString, "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes);
                if (result == MessageBoxResult.No) this.Close();
            }
            if(!_loggedIn.EditStudentAttendance(_student, _unit.unit, lectureAttendance, practicalAttendance))
            {
                var msgResult = MessageBox.Show("An error occured with editing the database, retry?", "Database Error", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (msgResult == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.IsEnabled = true;
        }
    }
}
