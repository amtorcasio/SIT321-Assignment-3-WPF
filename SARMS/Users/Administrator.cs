using System;
using System.Data.SQLite;
using System.Net.Mail;
using System.Collections.Generic;
using SARMS.Content;

namespace SARMS.Users
{
    public class Administrator : Account
    {
        //Constructor
        public Administrator(string id, string firstName, string lastName, string email, string password) :
            base(id, firstName, lastName, email, password)
        { }

        #region Public Methods
        //todo: SALT AND HASH PASSWORDS
        public void AddUser(string id, string firstName, string lastName, string email, string pass, UserType type)
        {
            Account u = new Account(this) { ID = id };

            if(!DoesRecordExist(u))
            {
                var connection = Utilities.GetDatabaseSQLConnection();

                try
                {
                    connection.Open();

                    System.Diagnostics.Debug.Write("About to add user");

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO User (Id, FirstName, LastName, Type, Status, Password, Email) VALUES (@id, @firstName, @lastName, @type, @status, @password, @email)";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName",  lastName);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@status", 1);

                    /*
                    using (System.Security.Cryptography.MD5 hash_object = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] data = hash_object.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

                        string hashed_pass = null;
                        foreach (byte b in data)
                            hashed_pass += b.ToString("x2");

                        command.Parameters.AddWithValue("@password", hashed_pass);
                    }*/
                    command.Parameters.AddWithValue("@password", pass);
                    command.Parameters.AddWithValue("@email", email);

                    command.ExecuteNonQuery();

                    System.Diagnostics.Debug.Write("Finished adding members");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("addUser Error: " + e.Message.ToString());
                    int count = 1;
                    Exception error = e.InnerException;
                    while (error != null)
                    {
                        System.Diagnostics.Debug.WriteLine("addUser Nested Error " + count + ": " + error.Message.ToString());
                        error = error.InnerException;
                        count++;
                    }
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
        }

        public bool SuspendUser(Account account)
        {
            if (DoesRecordExist(account))
            {
                var connection = Utilities.GetDatabaseSQLConnection();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Users SET Status = @status WHERE Id = @id";
                    command.Parameters.AddWithValue("@status", 0);
                    command.Parameters.AddWithValue("@id", account.ID);

                    return command.ExecuteNonQuery() == 0 ? false : true;
                }
                finally
                {
                    if (connection != null) connection.Close();
                }
            }
            return false;
        }

        public bool EditUser(Account account, string firstName, string lastName, string email, string password)
        {
            if (DoesRecordExist(account))
            {
                var connection = Utilities.GetDatabaseSQLConnection();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Users SET FirstName = @firstname, LastName = @lastname, Email = @email, Password = @password WHERE Id = @id";
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@email", email);
                    /*
                    using (System.Security.Cryptography.MD5 hash_object = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] data = hash_object.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                        string hashed_pass = null;
                        foreach (byte b in data)
                            hashed_pass += b.ToString("x2");

                        command.Parameters.AddWithValue("@password", hashed_pass);
                    }*/
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@id", account.ID);

                    return command.ExecuteNonQuery() == 0 ? false : true;
                }
                finally
                {
                    if (connection != null) connection.Close();
                }
            }
            else return false;
        }

        public bool RemoveUser(Account account)
        {
            if (DoesRecordExist(account))
            {
                var connection = Utilities.GetDatabaseSQLConnection();

                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Users WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", account.ID);

                    return command.ExecuteNonQuery() == 0 ? false : true;
                }
                finally
                {
                    if (connection != null) connection.Close();
                }
            }
            else return false;
        }

        public bool AddUnit(int id, string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        {
            //Do we need a DoesRecordExist function to check for existing units in its table?
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Unit (Id, Name, Code, Year, Trimester, TotalLectures, TotalPracticals) VALUES (@id, @name, @code, @year, @trimester, @totallect, @totalprac)";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@year", year); // datetime holds day,month, and year. but we only need year, and receiving input from the frontend is a string, not a datetime object (at least i don't think so)
                command.Parameters.AddWithValue("@trimester", trimester);
                command.Parameters.AddWithValue("@totallect", totalLectures);
                command.Parameters.AddWithValue("@totalprac", totalPracticals);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public bool EditUnit(Unit u, string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE TABLE Unit SET Name = @name, Code = @code, Year = @year, Trimester = @trimester, TotalLectures = @totallect, TotalPracticals = @totalprac WHERE Id = @id";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@year", year.Year);
                command.Parameters.AddWithValue("@trimester", trimester);
                command.Parameters.AddWithValue("@totallect", totalLectures);
                command.Parameters.AddWithValue("@totalprac", totalPracticals);
                command.Parameters.AddWithValue("@id", u.ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public void RemoveUnit(Unit unit)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                // set unit id for removal
                command.Parameters.AddWithValue("@uid", unit.ID);

                if (DoesRecordExist("SELECT 1 FROM UserUnits WHERE UnitID = " + unit.ID))
                {
                    // TABLE: UserUnits - Remove Records of Unit
                    command.CommandText = "DELETE FROM UserUnits WHERE UnitID = @uid";
                    command.ExecuteNonQuery();
                }

                if (DoesRecordExist("SELECT 1 FROM Assessments WHERE UnitID = " + unit.ID))
                {
                    // TABLE: Assessment - Remove Records of Unit
                    command.CommandText = "DELETE FROM Assessment WHERE UnitID = @uid";
                    command.ExecuteNonQuery();
                }

                if (DoesRecordExist(unit))
                {
                    // TABLE: Unit - Remove Records of Unit
                    command.CommandText = "DELETE FROM Unit WHERE ID = @uid";
                    command.ExecuteNonQuery();
                }

            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        // enroll student to unit
        public bool AddStudentUnit(Student student, Unit unit)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText =   "INSERT INTO UserUnits" +
                                        "([UserID],[UnitID],[LectureAttendance],[PracticalAttendance],[StaffFeedback],[StudentFeedback])" +
                                        "VALUES( @sid, @unitid, 0, 0 )";

                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        // withdraw student from unit
        public bool RemoveStudentUnit(Student student, Unit unit)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "DELETE FROM UserUnits WHERE UserID = @sid AND UnitID = @unitid";
                command.Parameters.AddWithValue("@sid", student.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        // allocate lecturer to unit
        public bool AddLecturerUnit(Lecturer lecturer, Unit unit)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "INSERT INTO UserUnits" +
                                        "([UserID],[UnitID],[LectureAttendance],[PracticalAttendance],[StaffFeedback],[StudentFeedback])" +
                                        "VALUES( @lid, @unitid )";

                command.Parameters.AddWithValue("@lid", lecturer.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        // lecturer deallocated from unit
        public bool RemoveLecturerUnit(Lecturer lecturer, Unit unit)
        {
            var connection = Utilities.GetDatabaseSQLConnection();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "DELETE FROM UserUnits WHERE UserID = @lid AND UnitID = @unitid";
                command.Parameters.AddWithValue("@lid", lecturer.ID);
                command.Parameters.AddWithValue("@unitid", unit.ID);

                return command.ExecuteNonQuery() == 0 ? false : true;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public Account SearchAccountsById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Account> SearchAccountsByUnit(Unit unit)
        {
            List<Account> result = null;
            return result;
        }

        public List<Unit> SearchUnits(string unitCode)
        {
            throw new NotImplementedException();
        }
        
        public bool DoesRecordExist(Account account)
        {
            return DoesRecordExist(@"SELECT 1 FROM User WHERE Id = '" + account.ID + "'");
        }

        public bool DoesRecordExist(Unit unit)
        {
            return DoesRecordExist(@"SELECT 1 FROM Unit WHERE Id = " + unit.ID);
        }
        #endregion

        //Private methods
        private bool DoesRecordExist(string queryString)
        {
            var connection = Utilities.GetDatabaseSQLConnection();
            SQLiteDataReader reader = null;

            try
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                reader = command.ExecuteReader();

                int recordCount = 0;
                while (reader.Read())
                {
                    recordCount++;
                }

                if (recordCount > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }
        }
    }
}
