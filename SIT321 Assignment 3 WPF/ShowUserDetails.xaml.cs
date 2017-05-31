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
using System.Windows.Navigation;
using System.Windows.Shapes;

using SARMS.Users;

namespace SIT321_Assignment_3_WPF
{
    /// <summary>
    /// Interaction logic for ShowUserDetails.xaml
    /// </summary>
    public partial class ShowUserDetails : UserControl
    {
        public ShowUserDetails(Account a)
        {
            InitializeComponent();
            
            lblName.Content = String.Format("{0}, {1}", a.LastName, a.FirstName);
            lblUsertype.Content = a.GetType().Name;
        }
    }
}
