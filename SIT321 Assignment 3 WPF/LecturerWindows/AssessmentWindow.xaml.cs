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
using SARMS.Content;

namespace SIT321_Assignment_3_WPF.LecturerWindows
{
    /// <summary>
    /// Interaction logic for AssessmentWindow.xaml
    /// </summary>
    public partial class AssessmentWindow : Window
    {
        private Assessment _assessment = null;
        private Unit _unit = null;
        private Window _from;

        private AssessmentWindow(Window from)
        {
            InitializeComponent();
            _from = from;
        }

        public AssessmentWindow(Window from, ref Assessment assessment)
            : this (from)
        {
            _assessment = assessment;

            btnSubmit.Content = "Submit Changes";
            this.Title = "Edit Assessment";
        }

        public AssessmentWindow(Window from, ref Unit unit)
            : this (from)
        {
            _unit = unit;

            btnSubmit.Content = "Create New Asessment";
            this.Title = "Create New Assessment";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _from.Show();
            _from.Focus();
        }
    }
}
