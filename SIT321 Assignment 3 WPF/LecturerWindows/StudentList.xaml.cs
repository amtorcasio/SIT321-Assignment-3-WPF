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

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for StudentList.xaml
    /// </summary>
    public partial class StudentList : Window
    {
        public StudentList(string windowTitle, string listTitle)
        {
            this.Title = windowTitle;
            gpoLabel.Text = listTitle;
        }

        //All Students At Risk
        public StudentList(Lecturer lecturer) :
            this ("List of Students At Risk (" + lecturer.Email + ")", 
                "SARs in units lectured by " + lecturer.FirstName + " " + lecturer.LastName)
        {
            InitializeComponent();
        }

        //All Students of a unit
        public StudentList(Unit unit):
            this ("List of Students", unit.Code + ": " + unit.Name)
        {
            InitializeComponent();
        }
    }
}
