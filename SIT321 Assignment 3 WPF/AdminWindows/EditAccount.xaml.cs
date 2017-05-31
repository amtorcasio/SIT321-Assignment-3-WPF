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
using System.Net.Mail;

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for EditAccount.xaml
    /// </summary>
    public partial class EditAccount : Window
    {
        // class instance accounts
        private Administrator Admin;
        private Account Editee;
        private Account Original;

        // Edit Account window
        public EditAccount(Administrator admin, Account editee)
        {
            InitializeComponent();

            Admin = admin;          // make class admin equal to passed administrator
            Editee = editee;        // set account to be edited
            Original = editee;      // set account to be referenced on the course of the editing

            // fill combo box
            cboStatus.Items.Add(new ComboBoxItem().Content = "Suspended");
            cboStatus.Items.Add(new ComboBoxItem().Content = "Active");

            // set up form
            txtID.Text = Original.ID;
            txtFirstname.Text = Original.FirstName;
            txtLastname.Text = Original.LastName;
            txtEmail.Text = Original.Email;
            psbPassword.Password = Original.Password;
            cboStatus.SelectedIndex = 
        }
    }
}
