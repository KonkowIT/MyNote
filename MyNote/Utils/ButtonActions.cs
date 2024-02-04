using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MyNote.Utils
{
    public class ButtonActions
    {
        static string caption = "MyNote";
        static MessageBoxButton okButton = MessageBoxButton.OK;
        public static void LogOutCommand_MouseLeftButtonDown(MainWindow mainWindow, MouseButtonEventArgs e)
        {
            mainWindow.NotesList.Clear();
            UserCache.LoggedInUser = null;
            mainWindow.Close();
            mainWindow.DisplayLoginWindow();
        }
        public static void AddNewNoteBtn_MouseLeftButtonDown(MainWindow mainWindow, MouseButtonEventArgs e)
        {
            Note note = new Note();
            try
            {
                mainWindow.SelectedNoteContent.Document.Blocks.Clear();
                note.AddNoteToDb();
                mainWindow.NotesList.Add(note);
                mainWindow.UpdateListBox();
            }
            catch (Exception ex)
            {
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(ex.Message, caption, okButton, icon);
            }
        }
        public static void SaveButton_Click(MainWindow mainWindow, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(mainWindow.SelectedNoteName.Text))
            {
                string msg = "Note title cannot be empty!";
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(msg, caption, okButton, icon);
            }
            else if (mainWindow.SelectedNoteName.Text.Length > 255)
            {
                string msg = "Note title has to be shorter then 255 characters!";
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(msg, caption, okButton, icon);
            }

            mainWindow.selectedNote.Title = mainWindow.SelectedNoteName.Text;
            TextRange textRange = new TextRange(mainWindow.SelectedNoteContent.Document.ContentStart, mainWindow.SelectedNoteContent.Document.ContentEnd);
            string xamlContent;
            using (MemoryStream ms = new MemoryStream())
            {
                textRange.Save(ms, DataFormats.Xaml);
                xamlContent = Encoding.Default.GetString(ms.ToArray());
            }

            string text = textRange.Text.Trim();
            mainWindow.selectedNote.Content = text;
            mainWindow.selectedNote.XamlContent = xamlContent;
            mainWindow.selectedNote.UpdateNote();
        }
        public static void RemoveNoteBtn_MouseLeftButtonDown(MainWindow mainWindow, object sender, RoutedEventArgs e)
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
                            mainWindow._notesList.Remove(noteToRemove);
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
        public static void Click_Bold(TextRange rangeToChange, object sender, RoutedEventArgs e)
        {
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
        public static void Click_Italic(TextRange rangeToChange, object sender, RoutedEventArgs e)
        {
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
        public static void Click_Underline(TextRange rangeToChange, object sender, RoutedEventArgs e)
        {
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
        public static void Click_Strikethrough(TextRange rangeToChange, object sender, RoutedEventArgs e)
        {
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
        public static void Click_AlignLeft(TextRange rangeToChange, object sender, RoutedEventArgs e)
        {
            rangeToChange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }
        public static void Click_AlignCenter(TextRange rangeToChange, object sender, RoutedEventArgs e)
        {
            rangeToChange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }
        public static void Click_AlignRight(TextRange rangeToChange, object sender, RoutedEventArgs e)
        {
            rangeToChange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }
    }
}
