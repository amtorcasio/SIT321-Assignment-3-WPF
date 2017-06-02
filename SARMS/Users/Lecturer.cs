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
        public bool AddAssessment(Unit unit, Assessment assessment)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            SQLiteCommand command = null;
            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText =   "INSERT INTO [Assessment] ([Id],[Name],[TotalMarks],[Weight],[UnitID])" +
                                        "VALUES(@aid, @aname, @atotalm, @weight, @unitid)";
                command.Parameters.AddWithValue("@id", DBNull.Value);
                command.Parameters.AddWithValue("@aname", assessment.Name);
                command.Parameters.AddWithValue("@atotalm", assessment.TotalMarks);
                command.Parameters.AddWithValue("@weight", assessment.Weight);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                bool success = command.ExecuteNonQuery() == 0 ? false : true;

                if (success)
                {
                    // add assessment to unit after added to database
                    unit.Assessments.Add(assessment);
                    return success;
                }
                return false;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        // remove assessment from unit
        public bool RemoveAssessment(Unit unit, Assessment assessment)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Assessment WHERE Id = @id AND UnitID = @unitid";
                command.Parameters.AddWithValue("@id", assessment.AssessmentID);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                bool success = command.ExecuteNonQuery() == 0 ? false : true;

                if (success)
                {
                    // add assessment to unit after added to database
                    unit.Assessments.Remove(assessment);
                    return success;
                }
                return false;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        // add student performance to assessment
        public bool AddStudentPerformance(Student student, Assessment assessment, double mark)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText =   "INSERT INTO [UserAssessment] ([UserID],[AssessmentID],[Mark])" +
                                        "VALUES (@sid, @assid, @mark)";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@assid", assessment.AssessmentID);
                command.Parameters.AddWithValue("@mark", mark);
                bool success = command.ExecuteNonQuery() == 0 ? false : true;

                if (success)
                {
                    // compile StudentAssessment
                    Data.StudentAssessment TempStudentAssessment = new Data.StudentAssessment();
                    TempStudentAssessment.account = student;
                    TempStudentAssessment.Assessment = assessment;
                    TempStudentAssessment.Mark = mark;

                    // add student performance on assessment
                    student.Performance.Add(TempStudentAssessment);
                    return success;
                }
                return success;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        // edit student performance data
        public bool EditStudentPerformance(Student student, Assessment assessment, double mark)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText =   "UPDATE [UserAssessment]" +
                                        "SET [AssessmentID] = @assid," +
                                            "[Mark] = @mark" +
                                        "WHERE UserID = @sid";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@assid", assessment.AssessmentID);
                command.Parameters.AddWithValue("@mark", mark);
                bool success = command.ExecuteNonQuery() == 0 ? false : true;

                if (success)
                {
                    // edit student performance on assessment
                    student.Performance.Find(e => (e.Assessment.AssessmentID == assessment.AssessmentID)).Mark = mark;
                    return success;
                }
                return success;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        // boolean if the student attended the lecturer and practical or did not attend
        public bool AddStudentAttendance(Student student, Unit unit, bool didAttentLecture, bool didAttendPractical)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = "UPDATE [UserUnits] SET "+
                                             "[LectureAttendance] = [LectureAttendance] + @lectbool," +
                                             "[PracticalAttendance] = [PracticalAttendance] + @pracbool " +
                                             "WHERE UserID = @sid AND UnitID = @unitid";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                command.Parameters.AddWithValue("@lectbool", Convert.ToInt32(didAttentLecture));
                command.Parameters.AddWithValue("@pracbool", Convert.ToInt32(didAttendPractical));

                bool success = command.ExecuteNonQuery() == 0 ? false : true;

                if (success)
                {
                    // add student attendanceon on unit
                    student.Units.Find(e => (e.unit.ID == unit.ID)).LectureAttendance += (didAttentLecture ? 1 : 0);
                    student.Units.Find(e => (e.unit.ID == unit.ID)).PracticalAttendance += (didAttendPractical ? 1 : 0);
                    return success;
                }
                return success;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        // direct editing of values
        public bool EditStudentAttendance(Student student, Unit unit, int numLectures, int numPracticals)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = "UPDATE [UserUnits] SET " +
                                             "[LectureAttendance] = @lect," +
                                             "[PracticalAttendance] = @prac " +
                                             "WHERE UserID = @sid AND UnitID = @unitid";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);
                command.Parameters.AddWithValue("@lect", numLectures);
                command.Parameters.AddWithValue("@prac", numPracticals);

                bool success = command.ExecuteNonQuery() == 0 ? false : true;

                if (success)
                {
                    // edit student attendanceon on unit
                    student.Units.Find(e => (e.unit.ID == unit.ID)).LectureAttendance = numLectures;
                    student.Units.Find(e => (e.unit.ID == unit.ID)).PracticalAttendance = numPracticals;
                    return success;
                }
                return success;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        // view student at risk
        public List<Student> viewSAR(Unit unit)
        {
            // create empty return variable
            List<Student> SARs = new List<Student>();

            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM User INNER JOIN UserUnits ON User.Id = UserUnits.UserID WHERE UserUnits.UnitID = @unitID AND UserUnits.AtRisk = 1";
                command.Parameters.AddWithValue("@unitID", unit.ID);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student temp = new Student(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                    LoadStudent(ref temp);
                    SARs.Add(temp);
                }
                
                /*
                command.CommandText = "SELECT [UserID] FROM [UserUnits] WHERE UnitID = @unitid AND AtRisk = 1";
                command.Parameters.AddWithValue("@unitid", unit.ID);

                reader = command.ExecuteReader();

                // list to append at risk students
                List<string> StudentatRiskIDList = new List<string>();

                // get student at risk id list list
                while (reader.Read())
                {
                    StudentatRiskIDList.Add(reader[0].ToString());
                }
                reader = null;      // prepare reader for next query
                command = null;     // null the SQLiteCommand

                // select all students at risk
                foreach (string id in StudentatRiskIDList)
                {
                    command = connection.CreateCommand();   // create command
                    command.CommandText = "SELECT * FROM User WHERE Id = @sid";
                    command.Parameters.AddWithValue("@sid", id);

                    reader = command.ExecuteReader();
                    reader.Read();

                    Student student = new Student(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                    LoadStudent(ref student);
                    SARs.Add(student);

                    reader = null;      // prepare reader for next query
                    command = null;     // null the SQLiteCommand
                }

                connection.Close();
                */
            }
            finally
            {
                if (command != null) command.Dispose();
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
            return SARs;
        }

        public List<Account> SearchAccountsByUnit(Unit unit)
        {
            List<Account> result = new List<Account>();
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM User INNER JOIN UserUnits ON User.Id = UserUnits.UserID WHERE UserUnits.UnitID = @id";
                    command.Parameters.AddWithValue("@id", unit.ID);

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        switch ((UserType)Convert.ToInt32(reader[3]))
                        {
                            //UserType.Administrator should not happen
                            case UserType.Administrator:
                                break;
                            case UserType.Lecturer:
                                break;
                            case UserType.Student:
                                Student student = new Student(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                                LoadStudent(ref student);
                                result.Add(student);
                                break;
                            default:
                                return null;
                        }
                    }
                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
                return result;
            }
        }

    }
}
