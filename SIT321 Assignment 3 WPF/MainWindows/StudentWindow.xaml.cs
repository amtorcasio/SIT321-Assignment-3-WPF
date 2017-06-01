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
using SARMS;
using SARMS.Users;
using SARMS.Data;

namespace SIT321_Assignment_3_WPF.MainWindows
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public Student LoggedInAccount { get; private set; }


        public StudentWindow(Student student)
        {
            LoggedInAccount = student;
            InitializeComponent();
            lsbUnits.ItemsSource = student.Units;
        }

        private void ListItem_Clicked(object sender, RoutedEventArgs e)
        {
            if ((sender as ListBox).Name == lsbUnits.Name)
                btnShowReport.IsEnabled = true;
        }

        private void btnShowReport_Click(object sender, RoutedEventArgs e)
        {
            var readFeedbackWindow = new Student_Windows.ShowFeedback();
            readFeedbackWindow.Show();
            readFeedbackWindow.Focus();


            
        }
    }
}