using System.Windows;
using SIT321_Assignment_3_WPF.Users;

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
            Administrator admin = new Administrator("hello", "there", "general", "kenobi");

            admin.addUser("12235", "andrea", "michele", "a@t.com", "wut", User.UserType.Administrator);
        }
    }
}
