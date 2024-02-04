using MyNote.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MyNote
{
    public class User
    {
        public Guid Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        DbConnectors _dbConnectors = new DbConnectors();

        public User(string firstName, string lastName, string email, string login, string password)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
        }
        public User(Guid id, string firstName, string lastName, string email, string login, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
        }
        public List<Note> GetLoggedUserNotes()
        {
            DbConnectors _dbConnectors = new DbConnectors();
            string getNotesForLoggeedUser = $"SELECT * FROM Notes n JOIN UsersNotes un ON n.NoteId = un.Note WHERE un.[User] = '{UserCache.LoggedInUser.Id}' ORDER BY n.ModyficationDate DESC;";
            List<Note> list = _dbConnectors.SelectNotesQuery(getNotesForLoggeedUser);
            return list;
        }
    }
}
