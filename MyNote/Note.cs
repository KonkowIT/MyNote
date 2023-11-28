using MyNote.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    public class Note
    {
        public Guid Id { get; }
        public DateTime CreationDate { get; }
        public DateTime ModyficationnDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        DbConnectors _dbConnectors = new DbConnectors();
        public Note(string title, string content = "")
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
            ModyficationnDate = DateTime.Now;
            Title = title;
            Content = content;
        }
        public void UpdateNote(Guid id, string title, string content)
        {
            ModyficationnDate = DateTime.Now;
            Title = title.Trim();
            Content = content;
        }
        internal void AddNoteToDb()
        {
            string insertNotesTable = $"INSERT INTO Notes (NoteId, CreationDate, ModyficationnDate, Content) VALUES ('{this.Id}', '{this.CreationDate}', '{this.ModyficationnDate}', '{this.Content}');";
            string insertUsersNotesTable = $"INSERT INTO UsersNotes (User, Note) VALUES ('{UserCache.LoggedInUser.Id}', '{this.Id}');";
            
            int notesTbResult = _dbConnectors.RunInsertOrUpdateQuery(insertNotesTable);
            int notesUsersTbResult = _dbConnectors.RunInsertOrUpdateQuery(insertUsersNotesTable);

            if (notesTbResult != 1 || notesUsersTbResult != 1)
            {
                throw new DatabaseInsertException("Adding note to the database failed");
            }
        }
    }
}
