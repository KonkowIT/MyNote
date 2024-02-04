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
        internal Note selectedNote;
        internal ObservableCollection<Note> _notesList = new ObservableCollection<Note>();
        public event PropertyChangedEventHandler PropertyChanged;

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
            if (null == UserCache.LoggedInUser)
            {
                Application.Current.Shutdown();
                return;
            }
            InitializeMyNote();
        }
        private void InitializeMyNote()
        {
            DisplayedLoggeduser.Text = $"Hello {UserCache.LoggedInUser.FirstName}!";
            var loggedUserNotes = UserCache.LoggedInUser.GetLoggedUserNotes();
            foreach (Note n in loggedUserNotes)
            {
                _notesList.Add(n);
            }
            NotesListBox.ItemsSource = NotesList;
            SelectedNoteName.IsEnabled = false;
            SelectedNoteContent.IsEnabled = false;
        }

        internal void DisplayLoginWindow()
        {
            Login loginWindow = new Login();
            loginWindow.ShowInTaskbar = false;
            loginWindow.ShowDialog();
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        internal void UpdateListBox()
        {
            OnPropertyChanged(nameof(NotesList));
        }
        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ListBox listbox = (System.Windows.Controls.ListBox)sender;
            Note newSelection = listbox.SelectedItem as Note;

            if (newSelection != null)
            {
                if (SelectedNoteName.IsEnabled == false)
                {
                    SelectedNoteName.IsEnabled = true;
                }
                if (SelectedNoteContent.IsEnabled == false)
                {
                    SelectedNoteContent.IsEnabled = true;
                }
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

        private void LogOutCommand_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ButtonActions.LogOutCommand_MouseLeftButtonDown(this, e);
        }
        private void AddNewNoteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ButtonActions.AddNewNoteBtn_MouseLeftButtonDown(this, e);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonActions.SaveButton_Click(this, e);
        }
        private void RemoveNoteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ButtonActions.RemoveNoteBtn_MouseLeftButtonDown(this, sender, e);
        }
        private void Click_Bold(object sender, RoutedEventArgs e)
        {
            TextRange range = GetSelectedText();
            ButtonActions.Click_Bold(range, sender, e);
        }
        private void Click_Italic(object sender, RoutedEventArgs e)
        {
            TextRange range = GetSelectedText();
            ButtonActions.Click_Italic(range, sender, e);
        }
        private void Click_Underline(object sender, RoutedEventArgs e)
        {
            TextRange range = GetSelectedText();
            ButtonActions.Click_Underline(range, sender, e);
        }
        private void Click_Strikethrough(object sender, RoutedEventArgs e)
        {
            TextRange range = GetSelectedText();
            ButtonActions.Click_Strikethrough(range, sender, e);
        }
        private void Click_AlignLeft(object sender, RoutedEventArgs e)
        {
            TextRange range = GetSelectedText();
            ButtonActions.Click_AlignLeft(range, sender, e);
        }
        private void Click_AlignCenter(object sender, RoutedEventArgs e)
        {
            TextRange range = GetSelectedText();
            ButtonActions.Click_AlignCenter(range, sender, e);
        }
        private void Click_AlignRight(object sender, RoutedEventArgs e)
        {
            TextRange range = GetSelectedText();
            ButtonActions.Click_AlignRight(range, sender, e);
        }
    }
}
