using MyNote.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    public class Note : INotifyPropertyChanged
    {
        private static string newNoteTitle = "New note";
        public Guid Id { get; }
        public DateTime CreationDate { get; }
        public DateTime ModyficationnDate { get; set; }
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        private string _contnent;
        public string Content
        {
            get { return _contnent; }
            set
            {
                if (_contnent != value)
                {
                    _contnent = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }        
        public String XamlContent { get; set; }
        DbConnectors _dbConnectors = new DbConnectors();
        private static string _dateTimeIsoFormat = "yyyy-MM-dd HH:mm:ss";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Note()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
            ModyficationnDate = DateTime.Now;
            Title = newNoteTitle;
            Content = string.Empty;
            XamlContent = string.Empty;
        }
        public Note(Guid id, DateTime creationDate, DateTime modyficationnDate, string title, string content, string xaml)
        {
            Id = id;
            CreationDate = creationDate;
            ModyficationnDate = modyficationnDate;
            Title = title;
            Content = content;
            XamlContent = xaml;
        }

        public void UpdateNote()
        {
            ModyficationnDate = DateTime.Now;
            Title = this.Title.Trim();
            Content = this.Content.Trim();
            XamlContent = this.XamlContent.Trim();

            string updateNoteString = $"UPDATE Notes SET ModyficationDate = '{this.ModyficationnDate.ToString(_dateTimeIsoFormat)}', Title = '{this.Title}', Content = '{this.Content}', Xaml = '{this.XamlContent}' WHERE NoteId = '{this.Id}';";
            int updateNotesResult = _dbConnectors.RunInsertOrUpdateQuery(updateNoteString);

            if (updateNotesResult != 1)
            {
                throw new DatabaseUpdateException("Saving note failed!");
            }
        }
        internal void AddNoteToDb()
        {
            string insertNotesString = $"INSERT INTO Notes (NoteId, CreationDate, ModyficationDate, Title, Xaml) VALUES ('{this.Id}', '{this.CreationDate.ToString(_dateTimeIsoFormat)}', '{this.ModyficationnDate.ToString(_dateTimeIsoFormat)}', '{this.Title}', '{this.XamlContent}');";
            string insertUsersNotesString = $"INSERT INTO UsersNotes ([User], Note) VALUES ('{UserCache.LoggedInUser.Id}', '{this.Id}');";
            
            int notesTbResult = _dbConnectors.RunInsertOrUpdateQuery(insertNotesString);
            int notesUsersTbResult = _dbConnectors.RunInsertOrUpdateQuery(insertUsersNotesString);

            if (notesTbResult != 1 || notesUsersTbResult != 1)
            {
                throw new DatabaseInsertException("Adding note to the database failed");
            }
        }

        internal void RemoveNoteFromDb()
        {
            string removeUsersNotesString = $"DELETE FROM UsersNotes WHERE Note = '{this.Id}';";
            string removeNoteString = $"DELETE FROM Notes WHERE NoteId = '{this.Id}';";

            int rmUsersNotesTbResult = _dbConnectors.RunInsertOrUpdateQuery(removeUsersNotesString);
            int rmNotesResult = _dbConnectors.RunInsertOrUpdateQuery(removeNoteString);
            
            if (rmUsersNotesTbResult != 1 || rmNotesResult != 1)
            {
                throw new DatabaseInsertException("Removing note from the database failed");
            }
        }
    }
}
