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

namespace SIT321_Assignment_3_WPF.MainWindows
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow(Student student)
        {
            InitializeComponent();
        }
    }
}
