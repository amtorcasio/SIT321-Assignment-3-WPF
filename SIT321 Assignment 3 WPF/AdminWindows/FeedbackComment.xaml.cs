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

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for FeedbackComment.xaml
    /// </summary>
    public partial class FeedbackComment : Window
    {
        private Administrator Admin;

        private Account Student;
        private List<Unit> UnitsList;

        private Window _from;

        public FeedbackComment(Administrator admin, Account student, List<Unit> units, Window from)
        {
            InitializeComponent();

            // set window object variables to parameters inserted
            Admin = admin;
            Student = student;
            UnitsList = units;
            _from = from;

            // fill lstunits items
            int count = 0;
            foreach(Unit u in UnitsList)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = u.Code + ": " + u.Name;
                lbi.FontSize = 12;
                lbi.Padding = new Thickness(5, 5, 5, 5);

                switch (count % 2)
                {
                    case 0:
                        lbi.Background = System.Windows.Media.Brushes.LightGray;
                        break;
                    case 1:
                        lbi.Background = System.Windows.Media.Brushes.SlateGray;
                        break;
                    default:
                        return;
                }

                lstUnits.Items.Add(lbi);
                count++;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.Show();
            _from.Focus();
        }


    }
}
