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
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for StudentList.xaml
    /// </summary>
    public partial class StudentList : Window
    {
        private Window _from;
        private Lecturer _loggedIn;
        private Unit _context;
        private bool infoVisible = true;

        public StudentList(Lecturer loggedIn, string windowTitle, string listTitle, Window from)
        {
            InitializeComponent();
            this.Title = windowTitle;
            tboUnit.Text = listTitle;
            _from = from;
            _loggedIn = loggedIn;
            if (loggedIn.ID == "admin.sarms") btnAddAttendance.Visibility = Visibility.Collapsed;
            if (loggedIn.ID == "admin.sarms") btnAddPerformance.Visibility = Visibility.Collapsed;
            if (infoVisible) ToggleInfoVisibility();
            tboInfo.Visibility = Visibility.Visible;
        }

        //All Students At Risk
        public StudentList(Lecturer loggedIn, Window from) :
            this (loggedIn, "List of Students At Risk (" + loggedIn.Email + ")", 
                "SARs in units lectured by " + loggedIn.FirstName + " " + loggedIn.LastName, from)
        {
            _context = null;
            var students = new List<Student>();
            foreach (Unit u in loggedIn.Units)
            {
                students.AddRange(loggedIn.viewSAR(u).ConvertAll(i => (Student)i));
            }
            if (students.Count > 0)
            {
                lsvStudents.ItemsSource = students;
                lsvStudents.Visibility = Visibility.Visible;
                btnDetailFeedback.Visibility = Visibility.Visible;
                tboNoStudents.Visibility = Visibility.Hidden;
            }
            else
            {
                tboNoStudents.Text = "Congratulations. It appears no students in your units are at risk";
            }
        }

        //All Students of a unit
        public StudentList(Lecturer loggedIn, Unit unit, Window from):
            this (loggedIn, "List of Students (" + loggedIn.Email + ")", unit.Code + ": " + unit.Name, from)
        {
            _context = unit;
            var students = loggedIn.SearchAccountsByUnit(unit);
            if (students.Count > 0)
            {
                lsvStudents.ItemsSource = students;
                lsvStudents.Visibility = Visibility.Visible;
                tboNoStudents.Visibility = Visibility.Hidden;
                btnDetailFeedback.Visibility = Visibility.Visible;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.Show();
            _from.Focus();
        }

        private void lsvStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvStudents.SelectedIndex == -1)
            {
                tboPerformance.Text = "Select a student to view his/her marks";
                lsvPerformance.Visibility = Visibility.Collapsed;
                tboPerformance.Visibility = Visibility.Visible;
                tboInfo.Visibility = Visibility.Visible;
                if (_loggedIn.ID != "admin.sarms") btnAddAttendance.Visibility = Visibility.Visible;
                btnEditAttendance.Visibility = Visibility.Collapsed;
            }

            Student student = lsvStudents.SelectedItem as Student;
            if (student == null) return;

            var perf = student.Performance;
            var matchContext = CollectionViewSource.GetDefaultView(perf);
            if (_context != null)
            {
                matchContext.Filter = sa => ((sa as StudentAssessment).Assessment.unit.ID == _context.ID);
            }
            else
            {
                //Auto
                gvcUnitCode.Width = Double.NaN;
            }
            if (perf.Count == 0)
            {
                lsvPerformance.Visibility = Visibility.Collapsed;
                if (_loggedIn.ID != "admin.sarms") btnAddPerformance.Visibility = Visibility.Visible;
                btnEditPerformance.Visibility = Visibility.Hidden;
                tboPerformance.Visibility = Visibility.Visible;
                tboPerformance.Text = "No assessments marks are on record for the selected student";
            }
            else
            {
                lsvPerformance.ItemsSource = perf;
                lsvPerformance.Visibility = Visibility.Visible;
                tboPerformance.Visibility = Visibility.Collapsed;
                if (_loggedIn.ID != "admin.sarms") btnAddPerformance.Visibility = Visibility.Visible;
                btnEditPerformance.Visibility = Visibility.Visible;
            }

            var info = student.Units;
            if (_context != null)
            {
                info = info.Where(su => (su.unit.ID == _context.ID)).ToList();
            }
            if (info.Count == 0)
            {
                throw new Exception("This should never happen");
            }
            else if (info.Count == 1)
            {
                if (!infoVisible) ToggleInfoVisibility();
                lsvInfo.Visibility = Visibility.Hidden;
                tboInfo.Visibility = Visibility.Collapsed;
                var su = info.Single();

                Binding lecBinding = new Binding();
                lecBinding.Source = su;
                lecBinding.Path = new PropertyPath("LectureAttendance");
                lecBinding.Mode = BindingMode.TwoWay;
                lecBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(txtLecturesAttended, TextBox.TextProperty, lecBinding);

                Binding pracBinding = new Binding();
                pracBinding.Source = su;
                pracBinding.Path = new PropertyPath("PracticalAttendance");
                pracBinding.Mode = BindingMode.TwoWay;
                pracBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(txtPracticalsAttended, TextBox.TextProperty, pracBinding);

                txtPracticalsAttended.Text = su.PracticalAttendance.ToString();
                txtAtRisk.Text = (bool)su.AtRisk ? "Yes" : "No";
            }
            else
            {
                if (infoVisible) ToggleInfoVisibility();
                tboInfo.Visibility = Visibility.Collapsed;
                lsvInfo.Visibility = Visibility.Visible;
                lsvInfo.ItemsSource = info;
            }
            if (_loggedIn.ID != "admin.sarms") btnAddAttendance.Visibility = Visibility.Visible;
            btnEditAttendance.Visibility = Visibility.Visible;
        }

        private void OnNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void ToggleInfoVisibility()
        {
            if (infoVisible)
            {
                lblLectures.Visibility = Visibility.Collapsed;
                lblPracticals.Visibility = Visibility.Collapsed;
                lblAtRisk.Visibility = Visibility.Collapsed;

                txtLecturesAttended.Visibility = Visibility.Collapsed;
                txtPracticalsAttended.Visibility = Visibility.Collapsed;
                txtAtRisk.Visibility = Visibility.Collapsed;

                infoVisible = false;
            }
            else
            {
                lblLectures.Visibility = Visibility.Visible;
                lblPracticals.Visibility = Visibility.Visible;
                lblAtRisk.Visibility = Visibility.Visible;

                txtLecturesAttended.Visibility = Visibility.Visible;
                txtPracticalsAttended.Visibility = Visibility.Visible;
                txtAtRisk.Visibility = Visibility.Visible;

                infoVisible = true;
            }
        }

        private void btnAddAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (lsvStudents.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a student to change the attendence for", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            Student student = lsvStudents.SelectedItem as Student;
            var info = student.Units;
            StudentUnit studentUnit = null;
            if (_context == null)
            {
                if (info.Count == 1)
                {
                    studentUnit = info.Single();
                }
                else
                {
                    if (lsvInfo.SelectedIndex == -1)
                    {
                        MessageBox.Show("You must select a unit to add the attendance for", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        return;
                    }
                    studentUnit = (lsvInfo.SelectedItem as StudentUnit);
                }
            }
            else
            {
                if (student == null)
                {
                    throw new Exception("Student from list is null after cast. This should not happen");
                }
                studentUnit = student.Units.Where(su => (su.unit.ID == _context.ID)).Single();
            }

            var attendLecture = MessageBox.Show("Did " + student.LastName + " " + student.FirstName + "attend the lecture?",
                    "Lecture Attendance", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (attendLecture == MessageBoxResult.Cancel)
            {
                return;
            }

            var attendPractical = MessageBox.Show("Did " + student.LastName + " " + student.FirstName + "attend the practical?",
                "Practical Attendance", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (attendPractical == MessageBoxResult.Cancel)
            {
                return;
            }

            bool didAttendLecture = (attendLecture == MessageBoxResult.Yes);
            bool didAttendPractical = (attendPractical == MessageBoxResult.Yes);
            if (!_loggedIn.AddStudentAttendance(student, studentUnit.unit, didAttendLecture, didAttendPractical))
            {
                MessageBox.Show("An error occurred updating these details in the database", "Databse Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditAttendance_Click(object sender, RoutedEventArgs e)
        {
            var student = lsvStudents.SelectedItem as Student;
            if (student == null)
            {
                MessageBox.Show("Student Must be Selected!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }
            var info = student.Units;
            long unitID;
            if (_context != null)
            {
                unitID = info.Where(su => (su.unit.ID == _context.ID)).Single().unit.ID;
            }
            else
            {
                if (info.Count == 1)
                {
                    unitID = info.Single().unit.ID;
                }
                else
                {
                    if (lsvInfo.SelectedIndex == -1)
                    {
                        MessageBox.Show("Unit to change attendance for must be selected", "Error!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        return;
                    }
                    unitID = (lsvInfo.SelectedItem as StudentUnit).unit.ID;
                }
            }
            var editWindow = new EditAttendanceWindow(_loggedIn, this, ref student, unitID);
            editWindow.Show();
        }

        private void btnEditPerformance_Click(object sender, RoutedEventArgs e)
        {
            if (lsvPerformance.SelectedIndex == -1)
            {
                if (lsvPerformance.Items.Count == 1)
                {
                    lsvPerformance.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("You must select an assessment item before editing it", "Selection incomplete", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
            }

            var perf = lsvPerformance.SelectedItem as StudentAssessment;
            if (perf != null)
            {
                var editWindow = new EditPerformance(_loggedIn, this, ref perf);
                editWindow.Show();
            }
            else
            {
                throw new InvalidCastException("Error occured casting performance item. This should never happen");
            }
        }

        private void btnAddPerformance_Click(object sender, RoutedEventArgs e)
        {
            var student = lsvStudents.SelectedItem as Student;

            Window addPerfWin = null;
            if (_context == null)
            {
                addPerfWin = new EditPerformance(_loggedIn, this, ref student);
            }
            else
            {
                addPerfWin = new EditPerformance(_loggedIn, this, ref student, _context);
            }

            addPerfWin.Show();
            addPerfWin.Focus();
        }

        private void btnDetailFeedback_Click(object sender, RoutedEventArgs e)
        {
            if (lsvStudents.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a student first", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            var window = new AdminWindows.FeedbackComment(_loggedIn, lsvStudents.SelectedItem as Student, this);
            window.Show();
            window.Focus();
            this.Hide();
        }
    }
}
