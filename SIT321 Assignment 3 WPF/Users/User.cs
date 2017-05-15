using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT321_Assignment_3_WPF.Users
{
    class User
    {
        public int ID;
        public string Firstname;
        public string Lastname;
        public string Email;
        private string Password;
        public List<Unit> Units;

        public void changePassword()
        { }

        public void forgotPassword()
        { }
    }
}
