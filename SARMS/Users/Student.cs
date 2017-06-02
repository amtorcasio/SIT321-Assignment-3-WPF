using System.Collections.Generic;
using SARMS.Data;
using System.Collections.ObjectModel;

namespace SARMS.Users
{
    public class Student : Account
    {
        // constructor
        public Student(string id, string firstName, string lastName, string email, string password):
            base (id, firstName, lastName, email, password)
        { }

        public ObservableCollection<StudentAssessment> Performance;
        public List<StudentUnit> Units;

    }
}
