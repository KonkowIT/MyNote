using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows;
using System.Xml.Linq;

namespace MyNote.Utils
{
    internal class DbConnectors
    {
        private SettingsRetriever _settingsRetriever = new SettingsRetriever();
        public int RunInsertOrUpdateQuery(string query)
        {
            try
            {
                int rowsAffected = 0;
                ConnectionStringSettings connStringSett = _settingsRetriever.GetConnectionString("connString");
                using (SqlConnection connection = new SqlConnection(connStringSett.ConnectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            transaction.Rollback();
                            throw new DatabaseInsertException("Query insertion failed!");
                        }
                    }

                    transaction.Commit();
                    connection.Close();
                }

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException(ex);
            }
        }
        public int RunInsertOrUpdateQuery(IEnumerable<string> query)
        {
            int sum = 0;

            foreach (string item in query)
            {
                sum += RunInsertOrUpdateQuery(item);
            }

            return sum;
        }
        public List<User> SelectUsersQuery(string query)
        {
            try
            {
                List<User> users = new List<User>();
                ConnectionStringSettings connStringSett = _settingsRetriever.GetConnectionString("connString");
                using (SqlConnection connection = new SqlConnection(connStringSett.ConnectionString))
                {
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        using (var cmd = new SqlCommand(query, connection))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var id = Guid.Parse(reader.GetString(0));
                                    var fName = reader.GetString(1);
                                    var lName = reader.GetString(2);
                                    var email = reader.GetString(3);
                                    var login = reader.GetString(4);
                                    var pswd = reader.GetString(5);
                                    User user  = new User(id, fName, lName, email, login, pswd);
                                    users.Add(user);
                                }

                                reader.Close();
                            }

                            connection.Close();
                        }
                    }
                    else
                    {
                        throw new DatabaseConnectionException($"Connection state: {connection.State.ToString()}.");
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException(ex);
            }
        }
        public List<Note> SelectNotesQuery(string query)
        {
            try
            {
                List<Note> notes = new List<Note>();
                ConnectionStringSettings connStringSett = _settingsRetriever.GetConnectionString("connString");
                using (SqlConnection connection = new SqlConnection(connStringSett.ConnectionString))
                {
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        using (var cmd = new SqlCommand(query, connection))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var id = Guid.Parse(reader.GetString(0));
                                    var creationDate = reader.GetDateTime(1);
                                    var modyficationDate = reader.GetDateTime(2);
                                    var content = SqlDataReaderExtensions.SafeGetString(reader, 3);
                                    var title = reader.GetString(4);
                                    var xaml = SqlDataReaderExtensions.SafeGetString(reader, 5);
                                    Note note = new Note(id, creationDate, modyficationDate, title, content, xaml);
                                    notes.Add(note);
                                }

                                reader.Close();
                            }

                            connection.Close();
                        }
                    }
                    else
                    {
                        throw new DatabaseConnectionException($"Connection state: {connection.State.ToString()}.");
                    }
                }

                return notes;
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException(ex);
            }
        }
    }
}
