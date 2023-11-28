using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyNote.Utils
{
    internal class DbConnectors
    {
        private string connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\KK\\source\\repos\\MyNote\\MyNote\\MyNoteDB.mdf;Integrated Security=True";
        public int RunInsertOrUpdateQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new DatabaseInsertException("Query insertion failed!");
                        }

                        return rowsAffected;
                    }
                }
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
                using (var connection = new SqlConnection(connString))
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
                            }
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
    }
}
