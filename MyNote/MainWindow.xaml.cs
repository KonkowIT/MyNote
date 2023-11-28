using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyNote
{
    public partial class MainWindow : Window
    {
        public List<Note> NotesList { get; set; }
        string caption = "Registration";
        MessageBoxButton button = MessageBoxButton.OK;
        public MainWindow()
        {
            InitializeComponent();
            if (null == UserCache.LoggedInUser)
            {
                DisplayLoginWindow();
            }
            DataContext = UserCache.LoggedInUser;
        }
        public void LogOutCommand()
        {
            NotesList.Clear();
            UserCache.LoggedInUser = null;
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
            Note note = new Note("New note");
            try { 
                note.AddNoteToDb();
                NotesList.Add(note);
            }
            catch (Exception ex)
            {
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(ex.Message, caption, button, icon);
            }

        }
    }
}
