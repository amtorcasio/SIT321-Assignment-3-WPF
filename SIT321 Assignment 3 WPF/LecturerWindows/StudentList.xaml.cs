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
using System.Diagnostics;

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for StudentList.xaml
    /// </summary>
    public partial class StudentList : Window
    {
        private Window _from;
        private Unit _context;
        private bool infoVisible = false;

        public StudentList(string windowTitle, string listTitle, Window from)
        {
            InitializeComponent();
            this.Title = windowTitle;
            tboUnit.Text = listTitle;
            _from = from;
        }

        //All Students At Risk
        public StudentList(Lecturer lecturer, Window from) :
            this ("List of Students At Risk (" + lecturer.Email + ")", 
                "SARs in units lectured by " + lecturer.FirstName + " " + lecturer.LastName, from)
        {
            _context = null;
            var students = new List<Student>();
            foreach (Unit u in lecturer.Units)
            {
                students.AddRange(lecturer.viewSAR(u).ConvertAll(i => (Student)i));
            }
            if (students.Count > 0)
            {
                lsvStudents.ItemsSource = students;
                lsvStudents.Visibility = Visibility.Visible;
                tboNoStudents.Visibility = Visibility.Hidden;
            }
        }

        //All Students of a unit
        public StudentList(Lecturer loggedIn, Unit unit, Window from):
            this ("List of Students (" + loggedIn.Email + ")", unit.Code + ": " + unit.Name, from)
        {
            _context = unit;
            var students = loggedIn.SearchAccountsByUnit(unit);
            if (students.Count > 0)
            {
                lsvStudents.ItemsSource = students;
                lsvStudents.Visibility = Visibility.Visible;
                tboNoStudents.Visibility = Visibility.Hidden;
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
            }

            Student student = lsvStudents.SelectedItem as Student;
            if (student == null) return;

            var perf = student.Performance;
            if (_context != null)
            {
                perf = perf.Where(sa => (sa.Assessment.unit.ID == _context.ID)).ToList();
                gvcUnitCode.Width = 0;
            }
            else
            {
                //Auto
                gvcUnitCode.Width = Double.NaN;
            }
            if (perf.Count == 0)
            {
                lsvPerformance.Visibility = Visibility.Collapsed;
                tboPerformance.Visibility = Visibility.Visible;
                tboPerformance.Text = "No assessments marks are on record for the selected student";
            }
            else
            {
                lsvPerformance.ItemsSource = perf;
                lsvPerformance.Visibility = Visibility.Visible;
                tboPerformance.Visibility = Visibility.Collapsed;
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
                txtLecturesAttended.Text = su.LectureAttendance.ToString();
                txtPracticalsAttended.Text = su.PracticalAttendance.ToString();
                tboStaffFeedback.Text = su.StaffFeedback;
                tboStudentFeedback.Text = su.StudentFeedback;
                txtAtRisk.Text = (bool)su.AtRisk ? "Yes" : "No";
            }
            else
            {
                if (infoVisible) ToggleInfoVisibility();
                tboInfo.Visibility = Visibility.Collapsed;
                lsvInfo.Visibility = Visibility.Visible;
                lsvInfo.ItemsSource = info;
            }
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
                lblStaffFeedback.Visibility = Visibility.Collapsed;
                lblStudentFeedback.Visibility = Visibility.Collapsed;
                lblAtRisk.Visibility = Visibility.Collapsed;

                txtLecturesAttended.Visibility = Visibility.Collapsed;
                txtPracticalsAttended.Visibility = Visibility.Collapsed;
                tboStaffFeedback.Visibility = Visibility.Collapsed;
                tboStudentFeedback.Visibility = Visibility.Collapsed;
                txtAtRisk.Visibility = Visibility.Collapsed;

                infoVisible = false;
            }
            else
            {
                lblLectures.Visibility = Visibility.Visible;
                lblPracticals.Visibility = Visibility.Visible;
                lblStaffFeedback.Visibility = Visibility.Visible;
                lblStudentFeedback.Visibility = Visibility.Visible;
                lblAtRisk.Visibility = Visibility.Visible;

                txtLecturesAttended.Visibility = Visibility.Visible;
                txtPracticalsAttended.Visibility = Visibility.Visible;
                tboStaffFeedback.Visibility = Visibility.Visible;
                tboStudentFeedback.Visibility = Visibility.Visible;
                txtAtRisk.Visibility = Visibility.Visible;

                infoVisible = true;
            }
        }

        private void gboSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (gboInfo.Width < gboPerformance.Width) gboInfo.Width = gboPerformance.Width;
            if (gboPerformance.Width < gboInfo.Width) gboPerformance.Width = gboInfo.Width;
        }
    }
}
