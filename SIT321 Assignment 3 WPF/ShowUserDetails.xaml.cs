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

namespace SIT321_Assignment_3_WPF
{
    /// <summary>
    /// Interaction logic for ShowUserDetails.xaml
    /// </summary>
    public partial class ShowUserDetails : UserControl
    {
        public ShowUserDetails(string name, string type)
        {
            InitializeComponent();

            lblName.Content = name;
            lblUsertype.Content = type;
        }
    }
}
