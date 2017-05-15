using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT321_Assignment_3_WPF
{
    class User
    {
        public int ID;
        public string Firstname;
        public string Lastname;
        public string Email;
        private string Password;
    }

    class Administrator : User
    {

    }

    class Lecturer : User
    {

    }

    class Student : User
    {

    }
}
