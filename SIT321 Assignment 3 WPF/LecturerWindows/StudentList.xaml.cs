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
using System.ComponentModel;

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
            lblUnit.Content = listTitle;
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
            lsvStudents.ItemsSource = loggedIn.SearchAccountsByUnit(unit);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.Show();
            _from.Focus();
        }

        private void lsvStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lsvPerformance.ItemsSource = (lsvStudents.SelectedItem as Student).Performance;
        }
    }
}
