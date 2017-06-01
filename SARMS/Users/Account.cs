using System;
using System.Data.SQLite;
using SARMS.Content;
using SARMS.Data;
using System.Collections.Generic;

namespace SARMS.Users
{
    public class Account
    {
        //private fields
        protected string _ID;
        protected string _firstName;
        protected string _lastName;
        protected string _email;
        protected string _password;

        //public fields
        public static string PATH_DATA = Utilities.GetPathData();
        #region Getters and Setters
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

        //Constructors
        public Account(Administrator creator)
        { }
        protected Account(string id, string firstName, string lastName, string email, string password)
        {
            _ID = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
        }

        #region Public Methods
        public static Account Login(string email, string password)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;
            SQLiteCommand command = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM User WHERE email = @email AND Password = @password";
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    switch ((UserType)Convert.ToInt32(reader[3]))
                    {
                        case UserType.Administrator:
                            return new Administrator(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                        case UserType.Lecturer:
                            Lecturer lecturer = new Lecturer(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                            LoadLecturer(ref lecturer);
                            return lecturer;
                        case UserType.Student:
                            Student student = new Student(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                            LoadStudent(ref student);
                            return student;
                        default:
                            return null;
                    }
                }
                return null;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
        }

        protected static void LoadLecturer(ref Lecturer lecturer)
        {
            List<Unit> units = new List<Unit>();
            foreach (StudentUnit su in GetUserUnitInfo(lecturer))
            {
                units.Add(GetUnitInfo(su.unit.ID.ToString()));
            }
            lecturer.Units = units;
        }

        protected static void LoadStudent(ref Student student)
        {
            student.Units = GetUserUnitInfo(student);
            student.Performance = GetStudentPerformance(student, student.Units);
        }

        protected static List<StudentUnit> GetUserUnitInfo(Account user)
        {
            SQLiteConnection connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;
            SQLiteCommand command = null;
            List<StudentUnit> result = new List<StudentUnit>();

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM UserUnits WHERE UserID = @id";
                command.Parameters.AddWithValue("@id", user.ID);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Unit temp = GetUnitInfo(reader[1].ToString());

                    result.Add(new StudentUnit()
                    {
                        account = user,
                        unit = temp,
                        LectureAttendance = (reader[2] != DBNull.Value) ? Convert.ToInt32(reader[2]) : (int?)null,
                        PracticalAttendance = (reader[3] != DBNull.Value) ? Convert.ToInt32(reader[3]) : (int?)null,
                        StaffFeedback = (reader[4] != DBNull.Value) ? reader[4].ToString() : null,
                        StudentFeedback = (reader[5] != DBNull.Value) ? reader[5].ToString() : null
                    });
                }
            }
            finally
            {
                if (command != null) command.Dispose();
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }

            return result;
        }

        protected static Unit GetUnitInfo(string ID)
        {
            SQLiteConnection connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;
            SQLiteCommand command = null;
            Unit result = null;

            try
            {
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Unit WHERE Id = @id";
                command.Parameters.AddWithValue("@id", ID);

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    result = new Unit(
                        Convert.ToInt64(reader[0]),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        Convert.ToInt16(reader[3]),
                        Convert.ToByte(reader[4]),
                        Convert.ToInt32(reader[5]),
                        Convert.ToInt32(reader[6]));
                }
                result.Assessments = GetUnitAssessments(result);

                return result;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
        }

        protected static List<Assessment> GetUnitAssessments(Unit u)
        {
            List<Assessment> result = new List<Assessment>();
            SQLiteConnection connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Assessment WHERE UnitID = @id";
                command.Parameters.AddWithValue("@id", u.ID);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Assessment(Convert.ToInt64(reader[0]), reader[1].ToString(),
                        Convert.ToInt32(reader[2]), Convert.ToDecimal(reader[3]), u));
                }
                return result;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
        }

        protected static List<StudentAssessment> GetStudentPerformance(Student student, List<StudentUnit> units)
        {
            List<StudentAssessment> result = new List<StudentAssessment>();
            SQLiteConnection connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "SELECT Mark FROM UserAssessment WHERE UserID = @userID AND AssessmentID = @assID";
                command.Parameters.AddWithValue("@userID", student.ID);

                foreach (StudentUnit su in units)
                {
                    foreach (Assessment ass in su.unit.Assessments)
                    {
                        command.Parameters.AddWithValue("@assID", ass.AssessmentID);
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            result.Add(new StudentAssessment() { account = student, Assessment = ass, Mark = Convert.ToDecimal(reader[0]) });
                        }
                    }
                }

                return result;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (reader != null) connection.Close();
                if (connection != null) connection.Close();
            }
            
        }

        public bool ChangePassword(string password)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;

            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "UPDATE User SET Password = @password WHERE Id = @id";
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@id", _ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        public static bool ForgotPassword(string email)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "SELECT Password FROM User WHERE Email = @email";
                command.Parameters.AddWithValue("@email", email);

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    Utilities.SendMailMessageFromAdmin(email, "Password Restore", reader[0].ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        public bool AddFeedBack(Account by, Student student, Unit unit, string feedback)
        {
            if (feedback.Trim().Length == 0) return false;

            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                command = connection.CreateCommand();
                string feedBackType = (by is Administrator || by is Lecturer) ? "StaffFeedback" : "StudentFeedback";
                command.CommandText = "SELECT " + feedBackType + " FROM UserUnits WHERE UserID = @userID AND UnitID = @unitID";
                command.Parameters.AddWithValue("@userID", student.ID);
                command.Parameters.AddWithValue("@unitID", unit.ID);
                reader = command.ExecuteReader();

                string currentFeedback = "";
                if (reader.HasRows)
                {
                    reader.Read();
                    currentFeedback = reader[0].ToString();
                }

                if (currentFeedback.Length > 0) currentFeedback += "\n";

                command.CommandText = "UPDATE UserUnits SET " + feedBackType + " = @feedback WHERE UserID = @userID AND UnitID = @unitID";
                string newfeedback = currentFeedback + feedback;
                command.Parameters.AddWithValue("@feeback", newfeedback);

                if (command.ExecuteNonQuery() == 0)
                    return false;

                var studentUnit = student.Units.Find(e => (e.unit.ID == unit.ID));
                if (by is Administrator || by is Lecturer)
                {
                    studentUnit.StaffFeedback = newfeedback;
                }
                else
                {
                    studentUnit.StudentFeedback = newfeedback;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }

        public StudentUnit GetFeedback(Student student, Unit unit)
        {
            return student.Units.Find(e => (e.unit.ID == unit.ID));
        }

        public void GetFeedback(Student student, Unit unit, out string staffFeeback, out string studentFeedback)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "SELECT StaffFeedback, StudentFeedback FROM UserUnits WHERE UserID = @userID AND UnitID = @unitID";
                command.Parameters.AddWithValue("@userID", student.ID);
                command.Parameters.AddWithValue("@unitID", unit.ID);
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    staffFeeback = reader[0].ToString();
                    studentFeedback = reader[0].ToString();
                }

                staffFeeback = null;
                studentFeedback = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null) connection.Close();
            }
        }
        #endregion
    }
}
