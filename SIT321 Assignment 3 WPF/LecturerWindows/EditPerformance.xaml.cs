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
using SARMS.Content;
using System.Collections.ObjectModel;

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

        public EditPerformance (Lecturer loggedIn, Window from)
        {
            InitializeComponent();
            _loggedIn = loggedIn;
            _from = from;
            _from.IsEnabled = false;
            this.Focus();
        }

        public EditPerformance(Lecturer loggedIn, Window from, ref StudentAssessment performance)
            : this (loggedIn, from)
        {
            _performance = performance;
            txtMark.Text = _performance.Mark.ToString();
            txtTotalMark.Text = _performance.Assessment.TotalMarks.ToString();

            cboUnit.ItemsSource = new List<Unit>() { performance.Assessment.unit };
            cboUnit.SelectedIndex = 0;
            cboAssessment.ItemsSource = new List<Assessment>() { performance.Assessment };
            cboAssessment.SelectedIndex = 0;

            lblStudent.Content += " " + _performance.account.LastName + " " + _performance.account.FirstName;
        }

        public EditPerformance(Lecturer loggedIn, Window from, ref Student student)
            : this (loggedIn, from)
        {
            _student = student;

            cboUnit.ItemsSource = loggedIn.Units;
            cboUnit.SelectedIndex = 0;
            cboUnit.IsEnabled = true;

            cboAssessment.ItemsSource = (cboUnit.SelectedItem as Unit).Assessments;
            cboAssessment.SelectedIndex = 0;
            cboAssessment.IsEnabled = true;

            txtTotalMark.Text = (cboAssessment.SelectedItem as Assessment).TotalMarks.ToString();

            lblStudent.Content = student.LastName + " " + student.FirstName + " ";
            this.Title = "Add Performance";
        }

        public EditPerformance(Lecturer loggedIn, Window from, ref Student student, Unit unit)
            : this(loggedIn, from)
        {
            _student = student;

            cboUnit.Items.Add(unit);
            cboUnit.SelectedIndex = 0;
            cboUnit.IsEnabled = true;

            cboAssessment.ItemsSource = (cboUnit.SelectedItem as Unit).Assessments;
            cboAssessment.SelectedIndex = 0;
            cboAssessment.IsEnabled = true;

            txtTotalMark.Text = (cboAssessment.SelectedItem as Assessment).TotalMarks.ToString();

            lblStudent.Content = student.LastName + " " + student.FirstName + " ";
            this.Title = "Add Performance";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        { 
            if (_performance == null)
            {
                var result = _student.Performance.Where(a => (a.Assessment.AssessmentID == (cboAssessment.SelectedItem as Assessment).AssessmentID)).ToList();
                if (result.Count == 0)
                {
                    string errorString = string.Empty;
                    var unit = cboUnit.SelectedItem as Unit;
                    if (unit == null)
                    {
                        errorString += "You must select a unit";
                    }

                    var assessment = cboAssessment.SelectedItem as Assessment;
                    if (assessment == null)
                    {
                        if (!string.IsNullOrEmpty(errorString)) errorString += Environment.NewLine;
                        errorString += "You must select an assessment";
                    }

                    double mark;
                    if (!double.TryParse(txtMark.Text, out mark) && !(mark >= 0))
                    {
                        if (!string.IsNullOrEmpty(errorString)) errorString += Environment.NewLine;
                        errorString += "You must select an assessment";
                    }

                    if (!string.IsNullOrEmpty(errorString))
                    {
                        errorString += Environment.NewLine + Environment.NewLine + "Do you wish to try again?";
                        var msgResult = MessageBox.Show(errorString, "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes);
                        if (msgResult == MessageBoxResult.No) this.Close();
                        return;
                    }

                    if (!_loggedIn.AddStudentPerformance(_student, assessment, mark))
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
                    this.Close();
                }
                else
                {
                    MessageBox.Show("This student has already been assigned a mark for this assignment" + Environment.NewLine +
                        "Please use the edit function to edit existing scores", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                }
            }
            else
            {
                double mark;
                if (!double.TryParse(txtMark.Text, out mark) && !(mark >= 0))
                {
                    var msgResult = MessageBox.Show("Invalid mark value" + Environment.NewLine + Environment.NewLine + "Do you wish to try again?",
                        "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes);
                    if (msgResult == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                    return;
                }

                if (!_loggedIn.EditStudentPerformance(_performance.account as Student, _performance.Assessment, mark))
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
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.IsEnabled = true;
        }

        private void cboUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboAssessment.ItemsSource = null;
            cboAssessment.ItemsSource = (cboUnit.SelectedItem as Unit).Assessments;
        }

        private void cboAssessment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtTotalMark.Text = (cboAssessment.SelectedItem as Assessment).TotalMarks.ToString();
        }
    }
}
