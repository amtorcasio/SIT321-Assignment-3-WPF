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
        long originalID;

        public EditUnit(Administrator admin, Unit unitee)
        {
            InitializeComponent();

            Admin = admin;      // make class admin equal to passed administrator
            Unitee = unitee;
            originalID = unitee.ID;

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string formatfix = string.Empty;
            int CodeNumbers, Trimester, TotalLectures, TotalPracticals;
            string CodeNumStr;
            string CodeLetters;
            DateTime year;

            try
            {
                if ((!Regex.IsMatch(txtCodeLetters.Text, @"^[a-zA-Z]+$")) || (txtCodeLetters.Text.Count() != 3))
                {
                    formatfix += "\nFirst Textbox of Unit Code must be LETTERS ONLY and maximum 3 characters!";
                }
                if ((int.TryParse(txtCodeNumbers.Text, out CodeNumbers) == false) || (txtCodeNumbers.Text.Count() != 3))
                {
                    formatfix += "\nSecond Textbox of Unit Code must be NUMBERS ONLY and maximum 3 characters!";
                }
                if (!Regex.IsMatch(txtYear.Text, "^(19|20)[0-9][0-9]"))
                {
                    formatfix += "\nYear provided must be in appropriate full year format (e.g. " + DateTime.Now.Year.ToString() + ")!";
                }
                if ((int.TryParse(txtTrimester.Text, out Trimester) == false) || (Trimester < 1) || (Trimester > 3))
                {
                    formatfix += "\nTrimester must be 1, 2 or 3!";
                }
                if ((int.TryParse(txtTotalLectures.Text, out TotalLectures) == false) || (TotalLectures < 0) || (TotalLectures > 36))
                {
                    formatfix += "\nTotal Lectures must be a number and greater then 0 and less the 36!";
                }
                if ((int.TryParse(txtTotalPracticals.Text, out TotalPracticals) == false) || (TotalPracticals < 0) || (TotalPracticals > 36))
                {
                    formatfix += "\nTotal Practicals must be a number and greater then 0 and less the 36!";
                }
                year = Convert.ToDateTime("01/01/" + txtYear.Text);
                CodeLetters = txtCodeLetters.Text.ToUpper();
                CodeNumStr = txtCodeNumbers.Text;
            }
            catch (FormatException ex)
            {
                throw ex;
            }

            if (formatfix != string.Empty)
            {
                MessageBox.Show(formatfix, "The following must be fixed to add unit to database!");
                return;
            }
            else
            {
                Unit NewUnit = new Unit(0, txtName.Text.ToUpper().Trim(), (txtCodeLetters.Text.ToUpper() + CodeNumStr.ToString()),
                    int.Parse(txtYear.Text), Trimester, TotalLectures, TotalPracticals);

                if (Admin.DoesRecordExist(NewUnit))
                {
                    MessageBox.Show("Please alter Unit Code, Year and/or Trimester", "Unit already exists!");
                    return;
                }

                Admin.EditUnit(originalID, NewUnit);
                MessageBox.Show("Unit: " + NewUnit.Code + " successfuly added to database", "Success");
                Close();
            }
        }

        private void btnRemoveUnit_Click(object sender, RoutedEventArgs e)
        {
            Admin.RemoveUnit(Unitee);
        }
    }
}
