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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web;
using System.Diagnostics;
using SIT321_Assignment_3_WPF.LecturerWindows;

namespace SIT321_Assignment_3_WPF.MainWindows
{
    /// <summary>
    /// Interaction logic for LecturerWindow.xaml
    /// </summary>
    public partial class LecturerWindow : Window
    {
        private Lecturer LoggedIn;
        private static string emailUrl = "mailto:sarms.edu@gmail.com";

        public LecturerWindow(Lecturer lecturer)
        {
            InitializeComponent();
            LoggedIn = lecturer;
            this.Title += " (" + lecturer.Email + ")";
            if (lecturer.Units.Count == 0)
            {
                gboUnits.Visibility = Visibility.Hidden;
                txtbNoUnits.Visibility = Visibility.Visible;

                emailUrl += "?subject=Lecturer ID: '" + lecturer.ID + "' has no units listed and is requesting access";
                hypEmail.NavigateUri = new Uri(emailUrl);
            }
            else
            {
                lsvUnits.ItemsSource = lecturer.Units;
            }
        }

        private void OnNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void btnViewStudents_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new StudentList(LoggedIn, lsvUnits.SelectedItem as Unit, this);
            listWindow.Show();
            listWindow.Focus();
            this.Hide();
        }

        private void btnAddAssessment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lsvUnits_Loaded(object sender, RoutedEventArgs e)
        {
            if (lsvUnits.Items.Count > 0)
            {
                lsvUnits.SelectedIndex = 0;
            }
        }

        private void btnViewSAR_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new StudentList(LoggedIn, this);
            listWindow.Show();
            listWindow.Focus();
            this.Hide();
        }
    }
}
