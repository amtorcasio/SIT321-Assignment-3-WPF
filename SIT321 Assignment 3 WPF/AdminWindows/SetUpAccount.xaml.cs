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
        public SetUpAccount(Administrator Admin)
        {
            InitializeComponent();

            cboAccountType.DataContext = Enum.GetNames(typeof(UserType));
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
