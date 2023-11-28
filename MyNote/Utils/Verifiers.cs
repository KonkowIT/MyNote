using MyNote.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyNote
{
    internal class Verifiers
    {
        DbConnectors _dbConnectors;
        public static BrushConverter converter = new System.Windows.Media.BrushConverter();
        public Brush brush = (Brush)converter.ConvertFromString("#d18a8a");
        public Brush originalBrush = (Brush)converter.ConvertFromString("#F0F0F0");
        public void VerifyTextBox(TextBox sender, bool isLoginBox = false) 
        {
            if (String.IsNullOrEmpty(sender.Text)) {
                sender.Background = brush;
                throw new MissingTextFieldException($"Field is missing!");
            }

            if (isLoginBox)
            {
                VerifyIfValueExistInDatabase(sender, "Login");
            }

            sender.Background = originalBrush;
        }

        private void VerifyIfValueExistInDatabase(TextBox sender, string dbColumnName)
        {
            _dbConnectors = new DbConnectors();
            string checkQuery = $"SELECT * FROM Users WHERE {dbColumnName} = '{sender.Text}';";
            var resFromDb = _dbConnectors.SelectUsersQuery(checkQuery);

            if (resFromDb.Count > 0)
            {
                sender.Background = brush;
                throw new ValueNotExistingInDatabaseException($"This {dbColumnName.ToLower()} is already taken!");
            }

            sender.Background = originalBrush;
        }

        public List<User> VerifyLogOn(string login, string pswd)
        {
            _dbConnectors = new DbConnectors();
            string checkQuery = $"SELECT * FROM Users WHERE Login = '{login}' AND Password = '{pswd}';";
            var resFromDb = _dbConnectors.SelectUsersQuery(checkQuery);

            if (resFromDb.Count == 0)
            {
                throw new ValueNotExistingInDatabaseException("Incorrect login and/or password!");
            }

            return resFromDb;
        }

        internal void VerifyEmailBox(TextBox sender)
        {
            VerifyTextBox(sender);

            try
            {
                var emailAddress = new MailAddress(sender.Text);
            }
            catch
            {
                sender.Background = brush;
                throw new InvalidEmailFieldException("Email address is incorrect!");
            }

            VerifyIfValueExistInDatabase(sender, "Email");
            sender.Background = originalBrush;
        }

        internal void VerifyPasswordBox(PasswordBox sender)
        {
            if (sender.Password.Length < 8 && !sender.Password.Any(char.IsDigit))
            {
                sender.Background = brush;
                throw new InvalidPswdFieldException("Password is incorrect! (Use: at least 8 digits, icluding 1 number)");
            }

            sender.Background = originalBrush;
        }
    }
}
