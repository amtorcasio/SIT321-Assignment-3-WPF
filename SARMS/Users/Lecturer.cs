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

        // add assessment to unit
        public void AddAssessment(Unit unit, Assessment assessment)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText =   "INSERT INTO [Assessment] ([Id],[Name],[TotalMarks],[Weight],[UnitID])" +
                                        "VALUES(@aid, @aname, @atotalm, @weight, @unitid)";
                command.Parameters.AddWithValue("@id", assessment.AssessmentID);
                command.Parameters.AddWithValue("@aname", assessment.Name);
                command.Parameters.AddWithValue("@atotalm", assessment.TotalMarks);
                command.Parameters.AddWithValue("@weight", assessment.Weight);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                command.ExecuteNonQuery();

                // add assessment to unit after added to database
                unit.Assessments.Add(assessment);

                System.Diagnostics.Debug.Write("Assessment " + assessment.AssessmentID + " added");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AddAssessment Error: " + e.Message.ToString());
            }
        }

        // remove assessment from unit
        public void RemoveAssessment(Unit unit, Assessment assessment)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Assessment WHERE Id = @id AND UnitID = @unitid";
                command.Parameters.AddWithValue("@id", assessment.AssessmentID);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                command.ExecuteNonQuery();

                // remove assessment from unit after removal from database
                unit.Assessments.Remove(assessment);

                System.Diagnostics.Debug.Write("Assessment " + assessment.AssessmentID + " removed");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("RemoveAssessment Error: " + e.Message.ToString());
            }
        }

        // add student performance to assessment
        public void AddStudentPerformance(Student student, Assessment assessment, int mark)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText =   "INSERT INTO [UserAssessment] ([UserID],[AssessmentID],[Mark])" +
                                        "VALUES (@sid, @assid, @mark)";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@assid", assessment.AssessmentID);
                command.Parameters.AddWithValue("@mark", mark);
                command.ExecuteNonQuery();

                // compile StudentAssessment
                Data.StudentAssessment TempStudentAssessment = new Data.StudentAssessment();
                TempStudentAssessment.account = student;
                TempStudentAssessment.Assessment = assessment;
                TempStudentAssessment.Mark = mark;

                // add student performance on assessment
                student.Performance.Add(TempStudentAssessment);

                System.Diagnostics.Debug.Write("Student " + student.ID + ", assessment " + assessment.AssessmentID + " with mark " + mark + " added");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AddStudentPerformance Error: " + e.Message.ToString());
            }
        }


        public void EditStudentPerformance(Student student, Assessment assessment, int mark)
        { }

        // boolean if the student attended the lecturer and practical or did not attend
        public void AddStudentAttendance(Student student, Unit unit, bool didAttentLecture, bool didAttendPractical)
        {
            //todo: add database side changes + practical changes
            student.Units.Find(e => (e.unit.ID == unit.ID)).LectureAttendance += (didAttentLecture ? 1 : 0);
            student.Units.Find(e => (e.unit.ID == unit.ID)).PracticalAttendance += (didAttendPractical ? 1 : 0);

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
