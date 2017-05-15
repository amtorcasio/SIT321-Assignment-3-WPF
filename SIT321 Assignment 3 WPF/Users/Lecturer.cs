namespace SIT321_Assignment_3_WPF.Users
{
    class Lecturer : User
    {
        // constructor
        public Lecturer(string fname, string lname, string email, string password)
        {
            Firstname = fname;
            Lastname = lname;
            Email = email;
            Password = password;
        }
    }
}
