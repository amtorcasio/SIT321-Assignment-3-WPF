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
            var usertypes = Enum.GetNames(typeof(UserType));
            foreach (string u in usertypes)
                cboAccountType.Items.Add(new ComboBoxItem().Content = u);

            // make class admin equal to logged in administrator
            Admin = temp;
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            // create attempt for new id
            string id = txtFirstname.Text[0] + txtLastname.Text.Substring(0, txtLastname.Text.Length);

            try
            {
                MailAddress m = new MailAddress(txtEmail.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Email is in incorrect format", "Formatting Issue");
                return;
            }

            // check if user id exists
            if (!Admin.DoesRecordExist(new Account(Admin) { ID = id }))
            {
                Admin.AddUser(id, txtFirstname.Text, txtLastname.Text, txtEmail.Text, psbPassword.Password, (UserType)Enum.Parse(typeof(UserType), cboAccountType.SelectedValue.ToString()));
                Close();    // close window
            }
            else
            {
                int Count = 1;
                while(Admin.SearchAccountsById(id+Count.ToString()) != null)
                {
                    Count++;
                }

                Admin.AddUser(id + Count, txtFirstname.Text, txtLastname.Text, txtEmail.Text, psbPassword.Password, (UserType)Enum.Parse(typeof(UserType), cboAccountType.SelectedValue.ToString()));
                Close();    // close window
            }
        }
    }
}
