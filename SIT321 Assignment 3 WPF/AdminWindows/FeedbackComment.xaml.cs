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
using System.Text.RegularExpressions;

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for FeedbackComment.xaml
    /// </summary>
    public partial class FeedbackComment : Window
    {
        private Account Admin;
        private Account Student;
        private Student ClassStudent;
        private List<Unit> UnitsList;

        private Window _from;

        SortedDictionary<DateTime, string[]> comments = new SortedDictionary<DateTime, string[]>();

        public FeedbackComment(Administrator admin, Account student, List<Unit> units, Window from)
        {
            InitializeComponent();

            // set window object variables to parameters inserted
            //Admin = admin;
            Student = student;
            UnitsList = units;
            _from = from;

            Student temp = new Student(Student.ID, Student.FirstName, Student.LastName, Student.Email, Student.Password);
            Account.LoadStudent(ref temp);

            ClassStudent = temp;

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

        public FeedbackComment(Lecturer lecturer, Student student, Window from)
        {
            InitializeComponent();

            //set window object variables to parameters inserted
            Admin = lecturer;
            Student = student;
            UnitsList = lecturer.Units.Intersect(student.Units.Select(u => u.unit)).ToList();
            _from = from;

            ClassStudent = student;

            // fill lstunits items
            int count = 0;
            foreach (Unit u in UnitsList)
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

        private void lstUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadFeedback();
        }

        private void loadFeedback()
        {
            int index = lstUnits.SelectedIndex;
            if (index >= 0)
            {
                lstFeedbackComments.Items.Clear();

                comments = new SortedDictionary<DateTime, string[]>();

                // get feedback
                string stafffeed, studentfeed;
                Admin.GetFeedback(Student, UnitsList[index], out stafffeed, out studentfeed);

                // split feedback
                List<string> stafftemp, studenttemp;

                if (stafffeed != string.Empty)
                {
                    stafftemp = stafffeed.Split('|').ToList();
                    foreach (string s in stafftemp)
                    {
                        string[] split = s.Split('<');
                        if (split[0] == string.Empty)
                            break;
                        for (int i = 0; i < split.Count() - 1; i++)
                        {
                            string stemp = split[i];
                            DateTime dtemp = DateTime.Parse(split[1]);
                            string[] values = { stemp, "staff" };
                            comments.Add(dtemp, values);
                        }
                    }
                }

                if (studentfeed != string.Empty)
                {
                    studenttemp = studentfeed.Split('|').ToList();
                    foreach (string s in studenttemp)
                    {
                        string[] split = s.Split('<');
                        if (split[0] == string.Empty)
                            break;
                        for (int i = 0; i < split.Count() - 1; i++)
                        {
                            string stemp = split[i];
                            DateTime dtemp = DateTime.Parse(split[1]);
                            string[] values = { stemp, "student" };
                            comments.Add(dtemp, values);
                        }
                    }
                }

                int count = 0;
                foreach (KeyValuePair<DateTime, string[]> commm in comments)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Content = commm.Key.ToString() + "\n" + commm.Value[0];
                    lbi.FontSize = 12;
                    lbi.Padding = new Thickness(5, 5, 5, 5);

                    if (commm.Value[1] == "student")
                    {
                        lbi.Background = System.Windows.Media.Brushes.LightBlue;
                    }
                    else
                    {
                        lbi.Background = System.Windows.Media.Brushes.LightSalmon;
                    }

                    lstFeedbackComments.Items.Add(lbi);
                    count++;
                }
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtSubmitText.Text.Trim().Count() > 0)
            {
                Admin.AddFeedBack(Admin, ClassStudent, UnitsList[lstUnits.SelectedIndex], txtSubmitText.Text);
                loadFeedback();
                txtSubmitText.Text = string.Empty;
            }
        }
    }
}
