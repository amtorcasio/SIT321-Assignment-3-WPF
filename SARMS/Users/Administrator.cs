﻿using System;
using System.Data.SQLite;
using System.Collections.Generic;
using SARMS.Content;
using System.Collections.ObjectModel;

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
                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    try
                    {
                        connection.Open();

                        System.Diagnostics.Debug.Write("About to add user");

                        command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO User (Id, FirstName, LastName, Type, Status, Password, Email) VALUES (@id, @firstName, @lastName, @type, @status, @password, @email)";
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@lastName", lastName);
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
                        if (command != null) command.Dispose();
                        if (connection != null)
                            connection.Close();
                    }
                }
            }
        }

        public bool SuspendUser(Account account)
        {
            if (DoesRecordExist(account))
            {
                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    try
                    {
                        connection.Open();

                        command = connection.CreateCommand();
                        command.CommandText = "UPDATE User SET Status = @status WHERE Id = @id";
                        command.Parameters.AddWithValue("@status", 0);
                        command.Parameters.AddWithValue("@id", account.ID);

                        return command.ExecuteNonQuery() == 0 ? false : true;
                    }
                    finally
                    {
                        if (command != null) command.Dispose();
                        if (connection != null) connection.Close();
                    }
                }
            }
            return false;
        }

        public bool chkifSAR(string accountid)
        {
            if (DoesRecordExist("SELECT 1 FROM UserUnits WHERE UserID = '" + accountid +"'"))
            {
                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    SQLiteDataReader reader = null;
                    try
                    {
                        connection.Open();

                        command = connection.CreateCommand();
                        command.CommandText = "SELECT AtRisk FROM UserUnits WHERE UserId = @id";
                        command.Parameters.AddWithValue("@id", accountid);

                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader[0] == DBNull.Value) continue;
                                if (int.Parse(reader[0].ToString()) == 1)
                                    return true;
                            }
                        }
                        return false;

                        //return command.ExecuteNonQuery() == 0 ? false : true;
                    }
                    finally
                    {
                        if (command != null) command.Dispose();
                        if (connection != null) connection.Close();
                    }
                }
            }
            else
            {
                return false;
            }
        }

        // un-suspend a user
        public bool ReactivateUser(Account account)
        {
            if (DoesRecordExist(account))
            {
                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    try
                    {
                        connection.Open();

                        command = connection.CreateCommand();
                        command.CommandText = "UPDATE User SET Status = @status WHERE Id = @id";
                        command.Parameters.AddWithValue("@status", 1);
                        command.Parameters.AddWithValue("@id", account.ID);

                        return command.ExecuteNonQuery() == 0 ? false : true;
                    }
                    finally
                    {
                        if (command != null) command.Dispose();
                        if (connection != null) connection.Close();
                    }
                }
            }
            return false;
        }

        // get type of user
        public int GetType(string accountid)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();
                    command.CommandText = "SELECT Type FROM User WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", accountid);

                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();

                        int result = -1;
                        int.TryParse(reader[0].ToString(), out result);

                        return result;
                    }
                    else
                    {
                        return -1;
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        // get user status a user
        public List<Unit> GetUnitsbyAccount(Account account)
        {
            List<long> unitids = new List<long>();
            List<Unit> returnunits = new List<Unit>();
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();
                    command.CommandText = "SELECT UnitID FROM UserUnits WHERE UserId = @id";
                    command.Parameters.AddWithValue("@id", account.ID);

                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            long result = -1;
                            long.TryParse(reader[0].ToString(), out result);

                            unitids.Add(result);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }

            foreach(long id in unitids)
            {
                returnunits.Add(GetUnit(id));
            }

            return returnunits;

        }

        // get user status a user
        public int GetStatus(Account account)
        {
            if (DoesRecordExist(account))
            {
                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    SQLiteDataReader reader = null;
                    try
                    {
                        connection.Open();

                        command = connection.CreateCommand();
                        command.CommandText = "SELECT Status FROM User WHERE Id = @id";
                        command.Parameters.AddWithValue("@id", account.ID);

                        reader = command.ExecuteReader();
                        reader.Read();

                        int result = -1;
                        int.TryParse(reader[0].ToString(), out result);

                        return result;
                    }
                    finally
                    {
                        if (reader != null) reader.Close();
                        if (command != null) command.Dispose();
                        if (connection != null) connection.Close();
                    }
                }
            }
            return -1;
        }

        public bool EditUser(Account account, string firstName, string lastName, string email, string password)
        {
            if (DoesRecordExist(account))
            {
                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    try
                    {
                        connection.Open();

                        command = connection.CreateCommand();
                        command.CommandText = "UPDATE User SET FirstName = @firstname, LastName = @lastname, Email = @email, Password = @password WHERE Id = @id";
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
                        if (command != null) command.Dispose();
                        if (connection != null) connection.Close();
                    }
                }
            }
            else return false;
        }

        public bool RemoveUser(Account account)
        {
            if (DoesRecordExist(account))
            {
                using (var connection = Utilities.GetDatabaseSQLConnection())
                {
                    SQLiteCommand command = null;
                    try
                    {
                        connection.Open();

                        command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM User WHERE Id = @aid";
                        command.Parameters.AddWithValue("@aid", account.ID);

                        return command.ExecuteNonQuery() == 0 ? false : true;
                    }
                    finally
                    {
                        if (command != null) command.Dispose();
                        if (connection != null) connection.Close();
                    }
                }
            }
            else return false;
        }

        public bool AddUnit(string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        {
            //Do we need a DoesRecordExist function to check for existing units in its table?
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Unit (Id, Name, Code, Year, Trimester, TotalLectures, TotalPracticals) VALUES (NULL, @name, @code, @year, @trimester, @totallect, @totalprac)";
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@code", code);
                    command.Parameters.AddWithValue("@year", year.Year); // datetime holds day,month, and year. but we only need year, and receiving input from the frontend is a string, not a datetime object (at least i don't think so)
                    command.Parameters.AddWithValue("@trimester", trimester);
                    command.Parameters.AddWithValue("@totallect", totalLectures);
                    command.Parameters.AddWithValue("@totalprac", totalPracticals);

                    return command.ExecuteNonQuery() == 0 ? false : true;
                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        public void EditUnit(long oldunitid, Unit newunit)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                try
                {
                    // begin process to delink old unit and link newunit
                    connection.Open();

                    command = connection.CreateCommand();
                    command.Parameters.AddWithValue("@uid", oldunitid);
                    command.Parameters.AddWithValue("@newname", newunit.Name);
                    command.Parameters.AddWithValue("@newcode", newunit.Code);
                    command.Parameters.AddWithValue("@newyear", Convert.ToDateTime("01/01/" + newunit.Year.ToString()) );
                    command.Parameters.AddWithValue("@newtri", newunit.Trimester);
                    command.Parameters.AddWithValue("@newlectol", newunit.TotalLectures);
                    command.Parameters.AddWithValue("@newpractol", newunit.TotalPracticals);

                    command.CommandText =   "UPDATE [Unit] SET "+
                                                   "[Name] = @newname, "+
                                                   "[Code] = @newcode, " +
                                                   "[Year] = @newyear, " +
                                                   "[Trimester] = @newtri, " +
                                                   "[TotalLectures] = @newlectol, " +
                                                   "[TotalPracticals] = @newpractol " +
                                                   "WHERE Id = @uid";
                    command.ExecuteNonQuery();
                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        public bool RemoveUnit(Unit unit)
        {
            bool removed = false;
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();

                    // set unit id for removal
                    command.Parameters.AddWithValue("@uid", unit.ID);

                    if (DoesRecordExist("SELECT 1 FROM UserUnits WHERE UnitID = " + unit.ID))
                    {
                        // TABLE: UserUnits - Remove Records of Unit
                        command.CommandText = "DELETE FROM UserUnits WHERE UnitID = @uid";
                        command.ExecuteNonQuery();
                        removed = true;
                    }

                    if (DoesRecordExist("SELECT 1 FROM Assessment WHERE UnitID = " + unit.ID))
                    {
                        // TABLE: Assessment - Remove Records of Unit
                        command.CommandText = "DELETE FROM Assessment WHERE UnitID = @uid";
                        command.ExecuteNonQuery();
                        removed = true;
                    }

                    if (DoesRecordExist("SELECT 1 FROM Unit WHERE ID = " + unit.ID))
                    {
                        command.CommandText = "DELETE FROM Unit WHERE ID = @uid";
                        command.ExecuteNonQuery();
                        removed = true;
                    }

                    return removed;

                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        // enroll student to unit
        public bool AddStudentUnit(Student student, Unit unit)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO UserUnits" +
                                            "([UserID],[UnitID],[LectureAttendance],[PracticalAttendance])" +
                                            "VALUES( @sid, @unitid, 0, 0 )";

                    command.Parameters.AddWithValue("@sid", student.ID);
                    command.Parameters.AddWithValue("@unitid", unit.ID);

                    return command.ExecuteNonQuery() == 0 ? false : true;
                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        // withdraw student from unit
        public bool RemoveStudentUnit(Student student, Unit unit)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();

                    command.CommandText = "DELETE FROM UserUnits WHERE UserID = @sid AND UnitID = @unitid";
                    command.Parameters.AddWithValue("@sid", student.ID);
                    command.Parameters.AddWithValue("@unitid", unit.ID);

                    if(command.ExecuteNonQuery() != 0)
                    {
                        command.CommandText = "DELETE FROM UserAssessment WHERE UserID = @sid";
                        if(command.ExecuteNonQuery() != 0)
                        {
                            return true;
                        }
                    }

                    return false;
                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        // allocate lecturer to unit
        public bool AddLecturerUnit(Lecturer lecturer, Unit unit)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO UserUnits" +
                                            "([UserID],[UnitID])" +
                                            "VALUES( @lid, @unitid )";

                    command.Parameters.AddWithValue("@lid", lecturer.ID);
                    command.Parameters.AddWithValue("@unitid", unit.ID);

                    return command.ExecuteNonQuery() == 0 ? false : true;
                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        // lecturer deallocated from unit
        public bool RemoveLecturerUnit(Lecturer lecturer, Unit unit)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                try
                {
                    connection.Open();

                    command = connection.CreateCommand();

                    command.CommandText = "DELETE FROM UserUnits WHERE UserID = @lid AND UnitID = @unitid";
                    command.Parameters.AddWithValue("@lid", lecturer.ID);
                    command.Parameters.AddWithValue("@unitid", unit.ID);

                    return command.ExecuteNonQuery() == 0 ? false : true;
                }
                finally
                {
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        public Account SearchAccountsById(string id)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;
                Account temp = new Account(this);

                try
                {
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM User WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

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
        }

        public bool isAccountEnrolled(long unitid, string userid)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();
                    command = connection.CreateCommand();

                    command.CommandText = "SELECT * FROM UserUnits WHERE UserID = @userid AND UnitID = @unitid";
                    command.Parameters.AddWithValue("@userid", userid);
                    command.Parameters.AddWithValue("@unitid", unitid);
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        return true;
                    }
                    else
                        return false;
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
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
                                result.Add(new Administrator(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString()));
                                break;
                            case UserType.Lecturer:
                                Lecturer lecturer = new Lecturer(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[6].ToString(), reader[5].ToString());
                                LoadLecturer(ref lecturer);
                                result.Add(lecturer);
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
                    if (reader != null) reader.Close();
                    if (command != null) command.Dispose();
                    if (reader != null) reader.Close();
                    if (connection != null) connection.Close();
                }
                return result;
            }
        }

        // get unit by id
        public Unit GetUnit(long unitid)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();
                    command = connection.CreateCommand();

                    command.CommandText = "SELECT * FROM Unit WHERE Id = @unitid";
                    command.Parameters.AddWithValue("@unitid", unitid);
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        Unit temp = new Unit(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), Convert.ToInt16(reader[3]), Convert.ToByte(reader[4]), Convert.ToInt32(reader[5]), Convert.ToInt32(reader[6]));
                        return temp;
                    }
                    else
                        return null;
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        // get unit by id
        public Unit GetLatestUnit(string unitcode)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();
                    command = connection.CreateCommand();

                    command.CommandText = "SELECT * FROM Unit WHERE Code = @unitcode ORDER BY Year DESC LIMIT 1";
                    command.Parameters.AddWithValue("@unitcode", unitcode);
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        Unit temp = new Unit(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), Convert.ToInt16(reader[3]), Convert.ToByte(reader[4]), Convert.ToInt32(reader[5]), Convert.ToInt32(reader[6]));
                        return temp;
                    }
                    else
                        return null;
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
        }

        public Unit SearchUnit(string unitCode, int year, int trimes)
        {
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();
                    command = connection.CreateCommand();

                    command.CommandText = "SELECT * FROM Unit WHERE Code = @unitcode AND Year = @uyear AND Trimester = @utri LIMIT 1";
                    command.Parameters.AddWithValue("@unitcode", unitCode);
                    command.Parameters.AddWithValue("@uyear", year);
                    command.Parameters.AddWithValue("@utri", trimes);
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        Unit temp = new Unit(Convert.ToInt64(reader[0]), reader[1].ToString(), reader[2].ToString(), Convert.ToInt16(reader[3]), Convert.ToByte(reader[4]), Convert.ToInt32(reader[5]), Convert.ToInt32(reader[6]));
                        temp.Assessments = new ObservableCollection<Assessment>(GetUnitAssessments(temp));
                        return temp;
                    }
                    return null;
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Close();
                }
            }
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
            using (var connection = Utilities.GetDatabaseSQLConnection())
            {
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;

                try
                {
                    connection.Open();
                    command = new SQLiteCommand(queryString, connection);
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
                    if (command != null) command.Dispose();

                    if (reader != null)
                        reader.Close();

                    if (connection != null)
                        connection.Close();
                }
            }
        }
    }
}
