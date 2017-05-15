using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

            Users.Administrator me = new Users.Administrator("gkol", "george", "kol", "me@me.com", "hello");
            me.addUser("gkolecs", me.Firstname, me.Lastname, me.Email, "hello", Users.User.UserType.Administrator);

            me.DoesRecordExist(me);
        }
    }
}
