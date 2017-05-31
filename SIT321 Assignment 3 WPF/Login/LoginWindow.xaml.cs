﻿using System;
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
        }
        public Account LoggedInAccount { get; private set; }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Account loggedInAccount = LoggedInAccount;
            string accountType = loggedInAccount.GetType().Name;
            Account result = Account.Login(txtEmail.Text, txtPassword.Password);
            if (result != null)
            {
                switch (accountType)
                {
                    case "Administrator":
                        var aWin = new AdminWindow(result);
                        this.Close();
                        aWin.Show();
                        aWin.Focus();
                        break;
                    case "Lecturer":
                        var lWin = new LecturerWindow(result);
                        this.Close();
                        lWin.Show();
                        lWin.Focus();
                        break;
                    case "Student":
                        var sWin = new StudentWindow(result);
                        this.Close();
                        sWin.Show();
                        sWin.Focus();
                        break;
                    default:
                        break;
                }
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
            var forgotWin = new ForgottenPasswordWindow();
            forgotWin.Show();
            forgotWin.Focus();
        }
    }
}
