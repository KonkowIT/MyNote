using MyNote.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace MyNote
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Note selectedNote;
        static string caption = "MyNote";
        static MessageBoxButton okButton = MessageBoxButton.OK;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Note> _notesList = new ObservableCollection<Note>();

        public ObservableCollection<Note> NotesList
        {
            get { return _notesList; }
            set
            {
                _notesList = value;
                OnPropertyChanged(nameof(NotesList));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            if (null == UserCache.LoggedInUser)
            {
                DisplayLoginWindow();
            }
            DisplayedLoggeduser.Text = $"Hello {UserCache.LoggedInUser.FirstName}!";
            var loggedUserNotes = GetLoggedUserNotes();
            foreach (Note n in loggedUserNotes)
            {
                _notesList.Add(n);
            }
            NotesListBox.ItemsSource = NotesList;
        }
        public void LogOutCommand_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _notesList.Clear();
            UserCache.LoggedInUser = null;
            Close();
            DisplayLoginWindow();
        }
        private void DisplayLoginWindow()
        {
            Login loginWindow = new Login();
            loginWindow.ShowInTaskbar = false;
            loginWindow.ShowDialog();
        }

        private void AddNewNoteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Note note = new Note();
            try 
            {
                SelectedNoteContent.Document.Blocks.Clear();
                note.AddNoteToDb();
                _notesList.Add(note);
                UpdateListBox();
            }
            catch (Exception ex)
            {
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(ex.Message, caption, okButton, icon);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateListBox()
        {
            OnPropertyChanged(nameof(NotesList));
        }

        internal static List<Note> GetLoggedUserNotes()
        {
            DbConnectors _dbConnectors = new DbConnectors();
            string getNotesForLoggeedUser = $"SELECT * FROM Notes n JOIN UsersNotes un ON n.NoteId = un.Note WHERE un.[User] = '{UserCache.LoggedInUser.Id}' ORDER BY n.ModyficationDate DESC;";
            List<Note> list = _dbConnectors.SelectNotesQuery(getNotesForLoggeedUser);
            return list;
        }

        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ListBox listbox = (System.Windows.Controls.ListBox)sender;
            Note newSelection = listbox.SelectedItem as Note;

            if (newSelection != null) 
            {
                selectedNote = newSelection;
                SelectedNoteName.Text = newSelection.Title;

                if (!string.IsNullOrEmpty(newSelection.XamlContent))
                {
                    try
                    {
                        SelectedNoteContent.Document.Blocks.Clear();
                        StringReader stringReader = new StringReader(newSelection.XamlContent);
                        XmlReader xmlReader = XmlReader.Create(stringReader);
                        Section sec = XamlReader.Load(xmlReader) as Section;
                        FlowDocument doc = new FlowDocument();

                        while (sec.Blocks.Count > 0)
                            doc.Blocks.Add(sec.Blocks.FirstBlock);

                        if (doc != null)
                        {
                            SelectedNoteContent.Document = doc;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to parse XAML content: {ex.Message}.");
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedNoteName.Text))
            {
                string msg = "Note title cannot be empty!";
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(msg, caption, okButton, icon);
            }
            else if (SelectedNoteName.Text.Length > 255) 
            {
                string msg = "Note title has to be shorter then 255 characters!";
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(msg, caption, okButton, icon);
            }

            selectedNote.Title = SelectedNoteName.Text;
            TextRange textRange = new TextRange(SelectedNoteContent.Document.ContentStart, SelectedNoteContent.Document.ContentEnd);
            string xamlContent;
            using (MemoryStream ms = new MemoryStream())
            {
                textRange.Save(ms, DataFormats.Xaml);
                xamlContent = Encoding.Default.GetString(ms.ToArray());
            }

            string text = textRange.Text.Trim();
            selectedNote.Content = text;
            selectedNote.XamlContent = xamlContent;
            selectedNote.UpdateNote();
        }

        private void RemoveNoteBtn_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Image image)
            {
                if (image.DataContext is Note noteToRemove)
                {
                    string desc = "Are you sure that you want to remove this note?\nThis operation cannot be reversed";
                    MessageBoxImage icon = MessageBoxImage.Question;
                    MessageBoxButton ynButton = MessageBoxButton.YesNo;
                    MessageBoxResult questResult = MessageBox.Show(desc, caption, ynButton, icon, MessageBoxResult.No);

                    if (questResult == MessageBoxResult.Yes)
                    {
                        try
                        {
                            noteToRemove.RemoveNoteFromDb();
                            _notesList.Remove(noteToRemove);
                        }
                        catch (Exception ex)
                        {
                            MessageBoxImage iconErr = MessageBoxImage.Error;
                            MessageBox.Show(ex.Message, caption, okButton, iconErr);
                        }
                    }
                }
            }
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Grid grid)
            {
                System.Windows.Controls.Image image = grid.Children.OfType<System.Windows.Controls.Image>().FirstOrDefault();
                if (image != null)
                {
                    image.Visibility = Visibility.Visible;
                }
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Grid grid)
            {
                System.Windows.Controls.Image image = grid.Children.OfType<System.Windows.Controls.Image>().FirstOrDefault();
                if (image != null)
                {
                    image.Visibility = Visibility.Collapsed;
                }
            }
        }

        private TextRange GetSelectedText()
        {
            TextRange range = new TextRange(SelectedNoteContent.Selection.Start, SelectedNoteContent.Selection.End);
            return range;
        }

        private void Click_Bold(object sender, RoutedEventArgs e)
        {
            TextRange rangeToChange = GetSelectedText();
            object fontWeight = rangeToChange.GetPropertyValue(TextElement.FontWeightProperty);

            if (fontWeight != null && fontWeight.Equals(FontWeights.Bold))
            {
                rangeToChange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                rangeToChange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
        }

        private void Click_Italic(object sender, RoutedEventArgs e)
        {
            TextRange rangeToChange = GetSelectedText();
            object fontStyle = rangeToChange.GetPropertyValue(TextElement.FontStyleProperty);

            if (fontStyle != null && fontStyle.Equals(FontStyles.Italic))
            {
                rangeToChange.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
            else
            {
                rangeToChange.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
        }

        private void Click_Underline(object sender, RoutedEventArgs e)
        {
            TextRange rangeToChange = GetSelectedText();
            object textDecorations = rangeToChange.GetPropertyValue(Inline.TextDecorationsProperty);

            if (textDecorations != null && ((TextDecorationCollection)textDecorations).Contains(TextDecorations.Underline[0]))
            {
                rangeToChange.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
            else
            {
                rangeToChange.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
        }

        private void Click_Strikethrough(object sender, RoutedEventArgs e)
        {
            TextRange rangeToChange = GetSelectedText();
            object textDecorations = rangeToChange.GetPropertyValue(Inline.TextDecorationsProperty);

            if (textDecorations != null && ((TextDecorationCollection)textDecorations).Contains(TextDecorations.Strikethrough[0]))
            {
                rangeToChange.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
            else
            {
                rangeToChange.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
            }
        }

        private void Click_AlignLeft(object sender, RoutedEventArgs e)
        {
            TextRange rangeToChange = GetSelectedText();
            rangeToChange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }

        private void Click_AlignCenter(object sender, RoutedEventArgs e)
        {
            TextRange rangeToChange = GetSelectedText();
            rangeToChange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }

        private void Click_AlignRight(object sender, RoutedEventArgs e)
        {
            TextRange rangeToChange = GetSelectedText();
            rangeToChange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }

        private void Click_ApplyBulletList(object sender, RoutedEventArgs e)
        {
            TextPointer caretPosition = SelectedNoteContent.CaretPosition;

            TextPointer start = caretPosition.GetLineStartPosition(0);
            TextPointer end = caretPosition.GetLineStartPosition(1) ?? caretPosition.DocumentEnd;

            Paragraph bulletParagraph = new Paragraph(new Run($"\u2022 {start.Paragraph}"));

            FlowDocument flowDocument = SelectedNoteContent.Document as FlowDocument;

            if (flowDocument != null)
            {
                flowDocument.Blocks.InsertBefore(start.Paragraph, bulletParagraph);
                flowDocument.Blocks.Remove(start.Paragraph);
            }
        }
    }
}
