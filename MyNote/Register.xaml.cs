using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyNote.Utils;

namespace MyNote
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        DbConnectors _dbConnectors;
        string caption = "Registration";
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxResult result = MessageBoxResult.OK;
        public Register()
        {
            InitializeComponent();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Verifiers verifiers = new Verifiers();
                verifiers.VerifyTextBox(txtFirstName);
                verifiers.VerifyTextBox(txtLastName);
                verifiers.VerifyEmailBox(txtEmail);
                verifiers.VerifyTextBox(txtLogin, true);
                verifiers.VerifyPasswordBox(pwdPassword);
                RegisterNewUser(txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtLogin.Text, pwdPassword.Password);
            }
            catch (Exception ex)
            { 
                MessageBoxImage icon = MessageBoxImage.Warning;
                var res = MessageBox.Show(ex.Message, caption, button, icon, result);
            }
        }
        private void RegisterNewUser(string fn, string ln, string email, string login, string pwd)
        {
            string insertQuery = $"INSERT INTO Users (UserId, FirstName, LastName, Email, Login, Password) VALUES ('{Guid.NewGuid()}', '{fn}', '{ln}', '{email}', '{login}', '{pwd}');";
            _dbConnectors= new DbConnectors();
            int isRegistered = _dbConnectors.RunInsertOrUpdateQuery(insertQuery);

            if (isRegistered == 1)
            {
                MessageBoxImage icon = MessageBoxImage.Asterisk;
                MessageBox.Show("Account registered successfuly!", caption, button, icon, result);
                Close();
            }
            else
            {
                throw new RegistrationFailedException("Registration failed. Try again!");
            }
        }
    }
}
