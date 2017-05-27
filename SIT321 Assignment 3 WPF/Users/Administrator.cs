using System;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.Generic;
using System.Data.SQLite.Linq;

namespace SIT321_Assignment_3_WPF.Users
{
    class Administrator : Account
    {
        // constructor
        public Administrator(string id, string fname, string lname, string email, string password)
        {
            ID = id;
            Firstname = fname;
            Lastname = lname;
            Email = email;
            Password = password;
        }

        //todo: SALT AND HASH PASSWORDS
        public void addUser(string id, string firstName, string lastName, string email, string pass, UserType type)
        {
            Account u = new Users.Account() { ID = id };
            if(!DoesRecordExist(u))
            {
                var connection = GetDatabaseSQLConnection();

                try
                {
                    connection.Open();

                    System.Diagnostics.Debug.Write("About to add user");

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO User (Id, FirstName, LastName, Type, Status, Password, Email) VALUES (@id, @firstName, @lastName, @type, @status, @password, @email)";
                    command.Parameters.Add(new SQLiteParameter("@id", id));
                    command.Parameters.Add(new SQLiteParameter("@firstName", firstName));
                    command.Parameters.Add(new SQLiteParameter("@lastName",  lastName));
                    command.Parameters.Add(new SQLiteParameter("@type", type));
                    command.Parameters.Add(new SQLiteParameter("@status", 1));

                    using (System.Security.Cryptography.MD5 hash_object = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] data = hash_object.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

                        string hashed_pass = null;
                        foreach (byte b in data)
                            hashed_pass += b.ToString("x2");

                        command.Parameters.Add(new SQLiteParameter("@password", hashed_pass));
                    }
                    //command.Parameters.Add(new SQLiteParameter("@password", pass));
                    command.Parameters.Add(new SQLiteParameter("@email", email));

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

        public void suspendUser(Account u)
        {
            if (DoesRecordExist(u))
            {
                //todo : add on to code snippet
                try
                {
                    var connection = GetDatabaseSQLConnection();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Users SET Status = @status WHERE Id = @id";
                    command.Parameters.Add(new SQLiteParameter("@status", 0));
                    command.Parameters.Add(new SQLiteParameter("@id", u.ID));

                    command.ExecuteNonQuery();
                    System.Diagnostics.Debug.Write("Member is suspended");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("suspendUser Error: " + e.Message.ToString());
                }
            }
        }

        public void terminateUser(Account u)
        {
            /* save a copy of the user's email,
             * execute SQL command to delete user entry from database,
             * alert user through email,
             * release copy of email
             */
        }

        public void editUser(Account u, string fname, string lname, string email, string pass)
        {
            if (DoesRecordExist(u))
            {
                //todo : add on to code snippet
                try
                {
                    var connection = GetDatabaseSQLConnection();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Users SET FirstName = @firstname, LastName = @lastname, Email = @email, Password = @password WHERE Id = @id";
                    command.Parameters.Add(new SQLiteParameter("@firstName", firstName));
                    command.Parameters.Add(new SQLiteParameter("@lastName", lastName));
                    command.Parameters.Add(new SQLiteParameter("@email", email));
                    using (System.Security.Cryptography.MD5 hash_object = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] data = hash_object.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

                        string hashed_pass = null;
                        foreach (byte b in data)
                            hashed_pass += b.ToString("x2");

                        command.Parameters.Add(new SQLiteParameter("@password", hashed_pass));
                    }
                    command.Parameters.Add(new SQLiteParameter("@id", u.ID));

                    command.ExecuteNonQuery();
                    System.Diagnostics.Debug.Write("Member data modified");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("editUser Error: " + e.Message.ToString());
                }
            }
        }

        public void removeUser(Account u)
        { }

        public void addUnit(string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        { }

        public void editUnit(Unit u, string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        { }

        public void removeUnit(Unit u)
        { }

        public void addStudentUnit(Student s, Unit u)
        { }

        public void removeStudentUnit(Student s, Unit u)
        { }

        public void addLecturerUnit(Lecturer lec, Unit u)
        { }
        
        public void removeLecturerUnit(Lecturer lec, Unit u)
        { }

        public void addFeedback(Student s, Unit u)
        { }

        public void getFeedback(Student s, Unit u)
        { }

        public void generateReport(Student s, Unit u)
        { }

        public bool DoesRecordExist(Account u)
        {
            return DoesRecordExist(@"SELECT 1 FROM User WHERE Id = '" + u.ID + "'");
        }

        private bool DoesRecordExist(string queryString)
        {
            var connection = GetDatabaseSQLConnection();
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DoesRecordExist Error: " + e.Message.ToString());
                int count = 1;
                Exception error = e.InnerException;
                while (error != null)
                {
                    System.Diagnostics.Debug.WriteLine("DoesRecordExist Nested Error " + count + ": " + error.Message.ToString());
                    error = error.InnerException;
                    count++;
                }
                return false;
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
