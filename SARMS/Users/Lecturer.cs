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

        // edit student performance data
        public void EditStudentPerformance(Student student, Assessment assessment, int mark)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText =   "UPDATE [UserAssessment]" +
                                        "SET [AssessmentID] = @assid," +
                                            "[Mark] = @mark" +
                                        "WHERE UserID = @sid";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@assid", assessment.AssessmentID);
                command.Parameters.AddWithValue("@mark", mark);
                command.ExecuteNonQuery();

                // edit student performance on assessment
                student.Performance.Find(e => (e.Assessment.AssessmentID == assessment.AssessmentID)).Mark = mark;

                System.Diagnostics.Debug.Write("Student " + student.ID + ", assessment " + assessment.AssessmentID + " with mark " + mark + " updated");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EditStudentPerformance Error: " + e.Message.ToString());
            }
        }

        // boolean if the student attended the lecturer and practical or did not attend
        public void AddStudentAttendance(Student student, Unit unit, bool didAttentLecture, bool didAttendPractical)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE [UserUnits] SET "+
                                             "[LectureAttendance] = [LectureAttendance] + @lectbool," +
                                             "[PracticalAttendance] = [PracticalAttendance] + @pracbool" +
                                             "WHERE UserID = @sid AND UnitID = @unitid";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                command.Parameters.AddWithValue("@lectbool", Convert.ToInt32(didAttentLecture));
                command.Parameters.AddWithValue("@pracbool", Convert.ToInt32(didAttendPractical));

                command.ExecuteNonQuery();

                // add student attendanceon on unit
                student.Units.Find(e => (e.unit.ID == unit.ID)).LectureAttendance += (didAttentLecture ? 1 : 0);
                student.Units.Find(e => (e.unit.ID == unit.ID)).PracticalAttendance += (didAttendPractical ? 1 : 0);

                System.Diagnostics.Debug.Write("Student " + student.ID + ", unit " + unit.ID + " AttendedLecture:" + didAttentLecture.ToString() + " AttendedPractical:" + didAttendPractical.ToString());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AddStudentAttendance Error: " + e.Message.ToString());
            }
        }

        // direct editing of values
        public void EditStudentAttendance(Student student, Unit unit, int numLectures, int numPracticals)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE [UserUnits] SET " +
                                             "[LectureAttendance] = @lect," +
                                             "[PracticalAttendance] = @prac" +
                                             "WHERE UserID = @sid AND UnitID = @unitid";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                command.Parameters.AddWithValue("@lect", numLectures);
                command.Parameters.AddWithValue("@prac", numPracticals);

                command.ExecuteNonQuery();

                // edit student attendanceon on unit
                student.Units.Find(e => (e.unit.ID == unit.ID)).LectureAttendance = numLectures;
                student.Units.Find(e => (e.unit.ID == unit.ID)).PracticalAttendance = numPracticals;

                System.Diagnostics.Debug.Write("Student " + student.ID + ", unit " + unit.ID + " LectureCount:" + numLectures.ToString() + " PracticalCount:" + numPracticals.ToString());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EditStudentAttendance Error: " + e.Message.ToString());
            }
        }

        public List<Account> viewSAR(Unit unit)
        {
            return new List<Account>();
        }
    }
}
