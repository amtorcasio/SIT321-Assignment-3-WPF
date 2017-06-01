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
using SARMS.Users;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web;

namespace SIT321_Assignment_3_WPF.MainWindows
{
    /// <summary>
    /// Interaction logic for LecturerWindow.xaml
    /// </summary>
    public partial class LecturerWindow : Window
    {
        private Lecturer LoggedIn;
        private static string emailUrl = "mailto:sarms.edu@gmail.com";

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header  
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lsvUnits.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        public LecturerWindow(Lecturer lecturer)
        {
            InitializeComponent();
            LoggedIn = lecturer;
            if (lecturer.Units.Count == 0)
            {
                lsvUnits.Visibility = Visibility.Hidden;
                txtbNoUnits.Visibility = Visibility.Visible;

                UriBuilder builder = new UriBuilder(emailUrl);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["subject"] = "Lecturer ID: " + lecturer.ID + " has no units listed and is requesting access";
                builder.Query = query.ToString();
                hypEmail.NavigateUri = builder.Uri;
            }
            else
            {
                lsvUnits.ItemsSource = lecturer.Units;
            }
        }

        private void lsvUnitsGVCH_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }
    }
}
