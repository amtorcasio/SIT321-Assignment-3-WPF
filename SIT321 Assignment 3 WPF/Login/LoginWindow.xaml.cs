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
using SIT321_Assignment_3_WPF.MainWindows;


namespace SIT321_Assignment_3_WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.Focus();

            //(new StudentWindows.ShowFeedback()).Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Account result = Account.Login(txtEmail.Text, txtPassword.Password);
            if (result != null)
            {
                Window nextWindow = null;
                if (result is Administrator)
                    nextWindow = new AdminWindow(result as Administrator);
                else if (result is Lecturer)
                    nextWindow = new LecturerWindow(result as Lecturer);
                else if (result is Student)
                    nextWindow = new StudentWindow(result as Student);

                this.Close(); // this.Hide();
                nextWindow.Show();
                nextWindow.Focus();
            }
            else
            {
                MessageBox.Show("Invalid login details", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            // usernameBox.Text -> username, passwordBox.Password -> password
        }

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var forgotWin = new ForgottenPasswordWindow(this);
            forgotWin.Show();
            forgotWin.Focus();
        }
    }
}
