using System;
using System.Data.SQLite;
using System.Net.Mail;
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

                    using (System.Security.Cryptography.MD5 hash_object = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] data = hash_object.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

                        string hashed_pass = null;
                        foreach (byte b in data)
                            hashed_pass += b.ToString("x2");

                        command.Parameters.AddWithValue("@password", hashed_pass);
                    }
                    //command.Parameters.AddWithValue("@password", pass));
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

        public void SuspendUser(Account a)
        {
            if (DoesRecordExist(a))
            {
                //todo : add on to code snippet
                var connection = Utilities.GetDatabaseSQLConnection();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Users SET Status = @status WHERE Id = @id";
                    command.Parameters.AddWithValue("@status", 0);
                    command.Parameters.AddWithValue("@id", a.ID);

                    command.ExecuteNonQuery();
                    System.Diagnostics.Debug.Write("Member is suspended");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("suspendUser Error: " + e.Message.ToString());
                }
            }
        }

        public void TerminateUser(Account a)
        {
            /* save a copy of the user's email,
             * execute SQL command to delete user entry from database,
             * alert user through email,
             * release copy of email
             */
             
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.google.com";

            MailMessage message = new MailMessage("admin@sarms.edu.au", a.Email);
            message.Subject = "SARMS - Notice of termination for user account ";
            message.Body = String.Format("Upon receiving this email, the account for user, {0} {1}, will be terminated and thus deleted from the SARMS database.", a.FirstName, a.LastName);
            client.Send(message);

            RemoveUser(a);
        }

        public void EditUser(Account acc, string firstName, string lastName, string email, string password)
        {
            if (DoesRecordExist(acc))
            {
                //todo : add on to code snippet
                var connection = Utilities.GetDatabaseSQLConnection();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Users SET FirstName = @firstname, LastName = @lastname, Email = @email, Password = @password WHERE Id = @id";
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@email", email);
                    using (System.Security.Cryptography.MD5 hash_object = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] data = hash_object.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                        string hashed_pass = null;
                        foreach (byte b in data)
                            hashed_pass += b.ToString("x2");

                        command.Parameters.AddWithValue("@password", hashed_pass);
                    }
                    command.Parameters.AddWithValue("@id", acc.ID);

                    command.ExecuteNonQuery();
                    System.Diagnostics.Debug.Write("Data for member " + acc.ID + " modified");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("editUser Error: " + e.Message.ToString());
                }
            }
        }

        public void RemoveUser(Account u)
        {
            // what in the world is this for?
        }

        public void AddUnit(int id, string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        {
            //Do we need a DoesRecordExist function to check for existing units in its table?
            //todo : add on to code snippet
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

                command.ExecuteNonQuery();
                System.Diagnostics.Debug.Write("New unit added");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("addUnit Error: " + e.Message.ToString());
            }
        }

        public void EditUnit(Unit u, string name, string code, DateTime year, int trimester, int totalLectures, int totalPracticals)
        {
            //todo : add on to code snippet
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

                command.ExecuteNonQuery();
                System.Diagnostics.Debug.Write("Unit " + name + " modified");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("editUnit Error: " + e.Message.ToString());
            }
        }

        public void RemoveUnit(Unit u)
        {
            /* check for associated classes
             * do not remove when associations are present
             */
        }

        public void AddStudentUnit(Student s, Unit u)
        {
            /* does Student and Unit exist in database?
             * add unit to Student's assigned Units
             * add relationship in database
             */
        }

        public void RemoveStudentUnit(Student s, Unit u)
        {
            /* does Student and Unit exist in database?
             * remove unit from Student's assigned Units
             * remove relationship in database
             */
        }

        public void AddLecturerUnit(Lecturer lec, Unit u)
        {
            /* does Lecturer and Unit exist in database?
             * add unit to Lecturer's assigned Units
             * add relationship in database
             */
        }

        public void RemoveLecturerUnit(Lecturer lec, Unit u)
        {
            /* does Lecturer and Unit exist in database?
             * remove unit from Lecturer's assigned Units
             * remove relationship in database
             */
        }

        public Account SearchAccountsById(int id)
        {
            throw new NotImplementedException();
        }

        public void SearchAccountsByUnit(Unit u)
        { }

        public void SearchUnits(string s)
        { }
        
        public bool DoesRecordExist(Account u)
        {
            return DoesRecordExist(@"SELECT 1 FROM User WHERE Id = '" + u.ID + "'");
        }

        public bool DoesRecordExist(Unit u)
        {
            return DoesRecordExist(@"SELECT 1 FROM Unit WHERE Id = " + u.ID);
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
