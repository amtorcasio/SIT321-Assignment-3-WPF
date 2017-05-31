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
using SARMS.Content;
using System.Net.Mail;

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for AddUnit.xaml
    /// </summary>
    public partial class AddUnit : Window
    {
        // class instance accounts
        private Administrator Admin;

        public AddUnit(Administrator admin)
        {
            InitializeComponent();

            Admin = admin;      // make class admin equal to passed administrator


        }
    }
}
