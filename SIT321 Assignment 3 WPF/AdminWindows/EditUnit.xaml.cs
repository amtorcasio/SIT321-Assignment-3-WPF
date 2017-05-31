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
using SARMS.Content;
using System.Text.RegularExpressions;

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for EditUnit.xaml
    /// </summary>
    public partial class EditUnit : Window
    {
        // class instance accounts
        private Administrator Admin;
        private Unit Unitee;

        public EditUnit(Administrator admin, Unit unitee)
        {
            InitializeComponent();

            Admin = admin;      // make class admin equal to passed administrator
            Unitee = unitee;

            // regex split unitcode
            var numAlpha = new Regex("(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
            var match = numAlpha.Match(Unitee.Code);

            var alpha = match.Groups["Alpha"].Value;
            var num = match.Groups["Numeric"].Value;

            // fill form
            txtName.Text = Unitee.Name;
            txtCodeLetters.Text = alpha;
            txtCodeNumbers.Text = num;
            txtYear.Text = Unitee.Year.ToString();
            txtTrimester.Text = Unitee.Trimester.ToString();
            txtTotalLectures.Text = Unitee.TotalLectures.ToString();
            txtTotalPracticals.Text = Unitee.TotalPracticals.ToString();
            


        }
    }
}
