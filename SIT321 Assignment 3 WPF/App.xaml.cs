using System.Windows;
using SARMS.Users;
using SARMS;

namespace SIT321_Assignment_3_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// //TEST
    public partial class App : Application
    {
        //APP ENTRY POINT AFTER base.OnStartup(e);
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Administrator admin = new Administrator("1", "hello", "there", "kenobi", "password");

            admin.AddUser("12345", "andrea", "michele", "a@t.com", "wut", UserType.Administrator);
        }
    }
}
