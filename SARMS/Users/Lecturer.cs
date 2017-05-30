using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SARMS.Content;

namespace SARMS.Users
{
    public class Lecturer : Account
    {
        public List<Unit> Units; // Lecturer.Units hides inherited member Acccount.Units

        // constructor
        public Lecturer(string id, string firstName, string lastName, string email, string password) :
            base (id, firstName, lastName, email, password)
        { }

        public void AddAssessment(Unit unit, Assessment assessment)
        {
            unit.Assessments.Add(assessment);


        }

        public void RemoveAssessment(Unit unit, Assessment assessment)
        {
            //todo:database side
            unit.Assessments.Remove(assessment);
        }

        public void AddStudentPerformance(Student student, Unit unit, Assessment assessment, int mark)
        { }

        public void EditStudentPerformance(Student student, Unit unit, Assessment assessment, int mark)
        { }

        // boolean if the student attended the lecturer and practical or did not attend
        public void AddStudentAttendance(Student student, Unit unit, bool didAttentLecture, bool didAttendPractical)
        {
            //todo: add database side changes + practical changes
            student.Units.Find(e => (e.unit.ID == unit.ID)).LectureAttendance += (didAttentLecture ? 1 : 0);

        }

        // direct editing of values
        public void EditStudentAttendance(Student student, Unit unit, int numLectures, int numPracticals)
        {

        }

        public List<Account> viewSAR(Unit unit)
        {
            return new List<Account>();
        }
    }
}
