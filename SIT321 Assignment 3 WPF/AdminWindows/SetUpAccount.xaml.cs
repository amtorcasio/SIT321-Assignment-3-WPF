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
            foreach (string s in usertypes)
                cboAccountType.Items.Add(s);

            // make class admin equal to logged in administrator
            Admin = temp;
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
