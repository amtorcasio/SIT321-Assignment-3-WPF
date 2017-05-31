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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Account LoggedInAccount { get; private set; }

        public MainWindow(Account loggedInAccount)
        {
            LoggedInAccount = loggedInAccount;
            InitializeComponent();
            
            lblName.Content = String.Format("{0}, {1} ", loggedInAccount.LastName.ToUpper(), loggedInAccount.FirstName.ToUpper());
            //if (loggedInAccount is Student)
                //if ((loggedInAccount as Student).AtRisk)
                    //lblName.Content += "(AT RISK)";

            ChangeUserControls(loggedInAccount.GetType().Name);
        }

        private void ChangeUserControls(string type)
        {
            gridBase.Children.Clear();

            // Does the new grid in the added UserControls overlap or replace the current grid in the window?
            switch (type)
            {
                case "Administrator":
                    gridBase.Children.Add(new AdministratorControls());
                    break;

                default:
                    break;
            }
        }
    }
}
