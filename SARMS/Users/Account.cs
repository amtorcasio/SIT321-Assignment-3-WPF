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

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
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
                            return LoginLecturer(connection, command, reader);
                        case UserType.Student:
                            return LoginStudent(connection, command, reader);
                        default:
                            return null;
                    }
                }
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection != null) connection.Close();
            }
        }

        private static Lecturer LoginLecturer(SQLiteConnection connection, SQLiteCommand command, SQLiteDataReader reader)
        {
            Lecturer lecturer = new Lecturer(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
            command.CommandText = "SELECT * FROM Unit INNER JOIN UserUnits ON Unit.Id = UserUnits.UnitID WHERE UserUnit.UserID = @id";
            command.Parameters.AddWithValue("@id", lecturer.ID);

            reader = command.ExecuteReader();

            List<Unit> units = new List<Unit>();

            SQLiteCommand unitAssessmentsCmd = connection.CreateCommand();
            unitAssessmentsCmd.CommandText = "SELECT * FROM Assessment WHERE UnitID = @id";
            SQLiteDataReader lecturerAssReader = null;


            while (reader.Read())
            {
                unitAssessmentsCmd.Parameters.AddWithValue("@id", Convert.ToInt32(reader[0]));
                List<Assessment> assessments = new List<Assessment>();
                Unit temp = new Unit(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(),
                    Convert.ToDateTime(reader[3]), Convert.ToInt32(reader[4]), Convert.ToInt32(reader[5]),
                    Convert.ToInt32(reader[6]));

                try
                {
                    lecturerAssReader = unitAssessmentsCmd.ExecuteReader();
                    while (lecturerAssReader.Read())
                    {
                        assessments.Add(new Assessment(Convert.ToInt32(lecturerAssReader[0]), lecturerAssReader[1].ToString(), Convert.ToInt32(lecturerAssReader[2]), Convert.ToDecimal(lecturerAssReader[3]), temp));
                    }
                    temp.Assessments = assessments;
                    units.Add(temp);
                }
                finally
                {
                    if (lecturerAssReader != null) lecturerAssReader.Close();
                }
            }

            lecturer.Units = units;
            return lecturer;
        }

        private static Student LoginStudent(SQLiteConnection connection, SQLiteCommand command, SQLiteDataReader reader)
        {
            Student student = new Student(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());

            command.CommandText = "SELECT * FROM UserAssessment WHERE UserID = @id";
            command.Parameters.AddWithValue("@id", student.ID);
            reader = command.ExecuteReader();
            List<StudentAssessment> performance = new List<StudentAssessment>();
            List<Unit> units = new List<Unit>();

            SQLiteCommand assessmentCmd = connection.CreateCommand();
            assessmentCmd.CommandText = "SELECT * FROM ASSESSMENT WHERE UnitID = @id";
            SQLiteDataReader studentAssReader = null;

            while (reader.Read())
            {
                assessmentCmd.Parameters.AddWithValue("@id", Convert.ToInt32(reader[1]));

                try
                {
                    studentAssReader = assessmentCmd.ExecuteReader();
                    studentAssReader.Read();

                    int unitID = Convert.ToInt32(studentAssReader[4]);
                    Unit unit = units.Find(e => (e.ID == unitID));
                    if (unit != null)
                    {
                        Assessment assessment = new Assessment(Convert.ToInt32(studentAssReader[0]), studentAssReader[1].ToString(),
                            Convert.ToInt32(studentAssReader[2]), Convert.ToDecimal(studentAssReader[3]), unit);
                        continue;
                    }

                    SQLiteCommand unitCommand = connection.CreateCommand();
                    unitCommand.CommandText = "SELECT * FROM UNIT WHERE Id = @id";
                    unitCommand.Parameters.AddWithValue("@id", unitID);
                    SQLiteDataReader unitReader = null;
                    try
                    {
                        unitReader = unitCommand.ExecuteReader();
                        unitReader.Read();

                        unit = new Unit(Convert.ToInt32(unitReader[0]), unitReader[1].ToString(), unitReader[2].ToString(),
                            Convert.ToDateTime(unitReader[3]), Convert.ToInt32(unitReader[4]), Convert.ToInt32(unitReader[5]),
                            Convert.ToInt32(unitReader[6]));

                        Assessment assessment = new Assessment(Convert.ToInt32(studentAssReader[0]), studentAssReader[1].ToString(),
                        Convert.ToInt32(studentAssReader[2]), Convert.ToDecimal(studentAssReader[3]), unit);

                    }
                    finally
                    {
                        if (unitReader != null) unitReader.Close();
                    }
                }
                finally
                {
                    if (studentAssReader != null) studentAssReader.Close();
                }
            }
            return student;
        }

        public bool ChangePassword(string password)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE User SET Password = @password WHERE Id = @id";
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@id", _ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public static bool ForgotPassword(string email)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
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
                if (connection != null) connection.Close();
            }
        }

        public bool AddFeedBack(Account by, Student student, Unit unit, string feedback)
        {
            if (feedback.Trim().Length == 0) return false;

            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
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
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
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
                if (connection != null) connection.Close();
            }
        }
        #endregion
    }
}
