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
            
        }

        //All Students of a unit
        public StudentList(Lecturer loggedIn, Unit unit, Window from):
            this ("List of Students", unit.Code + ": " + unit.Name, from)
        {
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
            }
            var perf = (lsvStudents.SelectedItem as Student).Performance;
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
        }

        private void OnNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
