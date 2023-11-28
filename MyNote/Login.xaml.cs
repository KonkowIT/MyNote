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
using System.Windows.Shapes;
using MyNote;

namespace MyNote
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        string caption = "Registration";
        MessageBoxButton button = MessageBoxButton.OK;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Verifiers verifiers = new Verifiers();
            string username = txtUsername.Text;
            string password = pwdPassword.Password;

            try
            {
                var loggedInUser = verifiers.VerifyLogOn(username, password)[0];
                UserCache.LoggedInUser = loggedInUser;
                Close();
            }
            catch (ValueNotExistingInDatabaseException ex)
            {
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(ex.Message, caption, button, icon);
            }
            catch (Exception ex) 
            {
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show($"Error: {ex.Message}", caption, button, icon);
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.Show();
        }
    }
}
