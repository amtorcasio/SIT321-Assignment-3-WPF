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
using System.Text.RegularExpressions;

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string formatfix = string.Empty;
            int CodeNumbers, Trimester, TotalLectures, TotalPracticals;
            string CodeLetters;
            DateTime year;
            string unitid;

            try {
                if ((!Regex.IsMatch(txtCodeLetters.Text, @"^[a-zA-Z]+$")) || (txtCodeLetters.Text.Count() != 3))
                {
                    formatfix += "\nFirst Textbox of Unit Code must be LETTERS ONLY and maximum 3 characters!";
                }
                if ((int.TryParse(txtCodeNumbers.Text, out CodeNumbers) == false) || (txtCodeNumbers.Text.Count() != 3))
                {
                    formatfix += "\nSecond Textbox of Unit Code must be NUMBERS ONLY and maximum 3 characters!";
                }
                if (!Regex.IsMatch(txtYear.Text, "^(19|20)[0-9][0-9]") || (int.Parse(txtYear.Text) < DateTime.Now.Year))
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
                year = Convert.ToDateTime("01/01/"+txtYear.Text);
                CodeLetters = txtCodeLetters.Text.ToUpper();
            }
            catch(Exception ex)
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
                // compile unit id
                unitid = (  (int)CodeLetters[0]).ToString() + ((int)CodeLetters[1]).ToString() + ((int)CodeLetters[2]).ToString() +
                    CodeNumbers.ToString() + Trimester.ToString() + int.Parse(txtYear.Text).ToString();

                long longunitid = long.Parse(unitid);

                Unit NewUnit = new Unit(longunitid, txtName.Text.ToUpper().Trim(), (txtCodeLetters.Text.ToUpper() + CodeNumbers.ToString()),
                    int.Parse(txtYear.Text), Trimester, TotalLectures, TotalPracticals);

                if(Admin.DoesRecordExist(NewUnit))
                {
                    MessageBox.Show("Unit already exists, please alter alpha and/or numeric section of Unit Code.");
                    return;
                }

                Admin.AddUnit(longunitid, NewUnit.Name, (txtCodeLetters.Text.ToUpper() + CodeNumbers.ToString()),
                    year, NewUnit.Trimester, NewUnit.TotalLectures, NewUnit.TotalPracticals);
                MessageBox.Show("Unit: " + NewUnit.Code + " successfuly added to database", "Success");
                Close();
            }
        }
    }
}
