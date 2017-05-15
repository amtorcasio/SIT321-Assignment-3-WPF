using System;

namespace SIT321_Assignment_3_WPF.Users
{
    class Student : User
    {
        public bool AtRisk { get; set; }
        public decimal Performance { get; set; }
        public int Attendance { get; set; }
    }
}
