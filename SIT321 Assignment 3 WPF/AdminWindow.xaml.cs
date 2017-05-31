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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public Account LoggedInAccount { get; private set; }

        public AdminWindow(Account lAccount)
        {
            LoggedInAccount = lAccount;
            InitializeComponent();
        }
    }
}
