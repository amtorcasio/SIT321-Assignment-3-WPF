using System;
using System.Data;
using System.Data.SqlClient;

namespace SIT321_Assignment_3_WPF.Users
{
    class Administrator : User
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
            User u = new Users.User() { ID = id };
            if(!DoesRecordExist(u))
            {
                SqlConnection connection = GetDatabaseSQLConnection();

                try
                {
                    connection.Open();

                    System.Diagnostics.Debug.Write("About to add user");

                    SqlCommand command = new SqlCommand("INSERT INTO [dbo].[User] (Id, FirstName, LastName, Type, Status, Password, Email)" +
                                                        "VALUES (@id, @firstName, @lastName, @type, @status, @password, @email)", connection);
                    command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    command.Parameters.Add("@firstName", SqlDbType.VarChar).Value = firstName;
                    command.Parameters.Add("@lastName", SqlDbType.VarChar).Value = lastName;
                    command.Parameters.Add("@type", SqlDbType.Int).Value = (int)type;
                    command.Parameters.Add("@status", SqlDbType.Int).Value = 1;
                    command.Parameters.Add("@password", SqlDbType.VarChar).Value = pass;
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

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

        public void suspendUser(User u)
        { }

        public void terminateUser(User u)
        { }

        public void editUser(User u, string fname, string lname, string email, string pass)
        { }

        public void removeUser(User u)
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

        public bool DoesRecordExist(User u)
        {
            return DoesRecordExist(@"SELECT 1 FROM User WHERE Id = '" + u.ID + "'");
        }

        private bool DoesRecordExist(string queryString)
        {
            SqlConnection connection = GetDatabaseSQLConnection();
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
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
