using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Libra
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        private LibraryLogic _libraryLogic;

        public BookWindow(LibraryLogic libraryLogic)
        {
            _libraryLogic = libraryLogic;
            InitializeComponent();
            InitializeBookList();
            UpdateBookCountLabel();
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateTitle();
            ValidateAuthor();
            ValidateISBN();
            if (ValidateTitle() && ValidateAuthor() && ValidateISBN())
            {
                if (_libraryLogic.GetBooks().Where(b => b.ISBN.Contains(BookISBNTextBox.Text)).Count() > 0)
                {
                    MessageBox.Show("A book with the same ISBN already exists.", "Duplicate ISBN", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Book newBook = new Book(BookTitleTextBox.Text, BookAuthorTextBox.Text, BookISBNTextBox.Text, BookState.Available);
                _libraryLogic.AddBook(newBook);
                InitializeBookList();
                UpdateBookCountLabel();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateBookCountLabel()
        {
            BookCountLabel.Content = $"Book Count: {_libraryLogic.GetBookCount()}";
        }

        private void InitializeBookList()
        {
            BookListBox.ItemsSource = _libraryLogic.GetBooks();
            BookListBox.Items.Refresh();
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                MenuButton.Content = item.Header; // Update the button content to show the selected menu item
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

            if (MenuButton.ContextMenu != null)
            {
                MenuButton.ContextMenu.PlacementTarget = MenuButton;
                MenuButton.ContextMenu.IsOpen = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        // delete selected book from the list and update the book count label
        {
            if (MenuButton.Content is string isbn)
            {
                _libraryLogic.RemoveBook(BookListBox.SelectedItem as Book);
                InitializeBookList();
                UpdateBookCountLabel();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text;
            string mode = MenuButton.Content?.ToString() ?? "AllFields";

            BookListBox.ItemsSource = _libraryLogic.SearchBooks(query, mode);
            BookListBox.Items.Refresh();
        }

        private void BookTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateTitle();
        }

        private bool ValidateTitle()
        {
            if (String.IsNullOrEmpty(BookTitleTextBox.Text))
            {
                BookTitleTextBox.Background = System.Windows.Media.Brushes.PaleVioletRed;
                BookTitleLabel2.Content = "Title can not be empty";
                BookTitleLabel2.Foreground = System.Windows.Media.Brushes.Red;
                return false;
            }
            BookTitleTextBox.Background = System.Windows.Media.Brushes.White;
            BookTitleLabel2.Content = "Title";
            BookTitleLabel2.Foreground = System.Windows.Media.Brushes.Black;
            return true;
        }

        private void BookAuthorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateAuthor();
        }

        private bool ValidateAuthor()
        {
            if (String.IsNullOrEmpty(BookAuthorTextBox.Text))
            {
                BookAuthorTextBox.Background = System.Windows.Media.Brushes.PaleVioletRed;
                BookAuthorLabel.Content = "Author can not be empty";
                BookAuthorLabel.Foreground = System.Windows.Media.Brushes.Red;
                return false;
            }
            BookAuthorTextBox.Background = System.Windows.Media.Brushes.White;
            BookAuthorLabel.Content = "Author";
            BookAuthorLabel.Foreground = System.Windows.Media.Brushes.Black;
            return true;
        }

        private void BookISBNTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateISBN();
        }

        private bool ValidateISBN()
        {
            string regexPattern = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";

            if (String.IsNullOrEmpty(BookISBNTextBox.Text))
            {
                BookISBNTextBox.Background = System.Windows.Media.Brushes.PaleVioletRed;
                ISBNLabel.Content = "ISBN can not be empty";
                ISBNLabel.Foreground = System.Windows.Media.Brushes.Red;
                return false;
            }
            else if (!Regex.IsMatch(BookISBNTextBox.Text, regexPattern))
            {
                BookISBNTextBox.Background = System.Windows.Media.Brushes.PaleVioletRed;
                ISBNLabel.Content = "Not a valid ISBN number";
                ISBNLabel.Foreground = System.Windows.Media.Brushes.Red;
                return false;
            }
            BookISBNTextBox.Background = System.Windows.Media.Brushes.White;
            ISBNLabel.Content = "ISBN";
            ISBNLabel.Foreground = System.Windows.Media.Brushes.Black;
            return true;
        }
    }
}
