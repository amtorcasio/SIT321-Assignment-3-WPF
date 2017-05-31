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
using System.Net.Mail;

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for SetUpAccount.xaml
    /// </summary>
    public partial class SetUpAccount : Window
    {
        private Administrator Admin;

        public SetUpAccount(Administrator temp)
        {
            InitializeComponent();

            // populate combobox
            cboAccountType.DataContext = Enum.GetNames(typeof(UserType));

            // make class admin equal to logged in administrator
            Admin = temp;
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            // create attempt for new id
            string id = txtFirstname.Text[0] + txtLastname.Text.Substring(0, 6);

            try
            {
                MailAddress m = new MailAddress(txtEmail.Text);
            }
            catch (FormatException)
            {
                lblStatus.Content = "Email is incorrect format";
                return;
            }

            // check if user id exists
            if (Admin.SearchAccountsById(id) == null)
            {
                Admin.AddUser(id, txtFirstname.Text, txtLastname.Text, txtEmail.Text, psbPassword.Password, (UserType)Enum.Parse(typeof(UserType), cboAccountType.SelectedValue.ToString()));
            }
            else
            {
                int Count = 1;
                while(Admin.SearchAccountsById(id+Count.ToString()) != null)
                {
                    Count++;
                }

                Admin.AddUser(id + Count, txtFirstname.Text, txtLastname.Text, txtEmail.Text, psbPassword.Password, (UserType)Enum.Parse(typeof(UserType), cboAccountType.SelectedValue.ToString()));
            }
        }
    }
}
