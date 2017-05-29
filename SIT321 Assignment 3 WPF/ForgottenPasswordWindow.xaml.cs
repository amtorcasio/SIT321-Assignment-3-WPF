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

namespace SIT321_Assignment_3_WPF
{
    /// <summary>
    /// Interaction logic for ForgottenPasswordWindow.xaml
    /// </summary>
    public partial class ForgottenPasswordWindow : Window
    {
        public ForgottenPasswordWindow()
        {
            InitializeComponent();
        }

        private void btnSendPassword_Click(object sender, RoutedEventArgs e)
        {
            if (Account.ForgotPassword(txtEmail.Text))
            {
                MessageBox.Show("An email containing your password has been sent", "Email sent", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                MessageBox.Show("An error occured sending you an email. Perhaps the email you entered does not exist in our system?", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
