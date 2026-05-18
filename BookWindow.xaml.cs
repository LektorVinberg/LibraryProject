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
            /*            ValidateTitle();
                        ValidateAuthor();
                        ValidateISBN();
                        AddBookButton.IsEnabled = (ValidateTitle() && ValidateAuthor() && ValidateISBN());
            */
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            ValidTitle();
            ValidAuthor();
            ValidISBN();
            if (ValidTitle() && ValidAuthor() && ValidISBN().result)
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
            else
            {
                AddBookButton.IsEnabled = false;
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
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete the book? This action can not be undone!",
                "Delete book",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (MenuButton.Content is string isbn)
                {
                    _libraryLogic.RemoveBook(BookListBox.SelectedItem as Book);
                    InitializeBookList();
                    UpdateBookCountLabel();
                }
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
            if (ValidTitle())
            {
                BookTitleTextBox.Background = System.Windows.Media.Brushes.White;
                BookTitleLabel2.Content = "Title";
                BookTitleLabel2.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                BookTitleTextBox.Background = System.Windows.Media.Brushes.PaleVioletRed;
                BookTitleLabel2.Content = "Title can not be empty";
                BookTitleLabel2.Foreground = System.Windows.Media.Brushes.Red;
            }
            AddBookButton.IsEnabled = (ValidTitle() & ValidAuthor() & ValidISBN().result);
        }


        private bool ValidTitle()
        {
            return !String.IsNullOrEmpty(BookTitleTextBox.Text);
        }

        private void BookAuthorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidAuthor())
            {
                BookAuthorTextBox.Background = System.Windows.Media.Brushes.White;
                BookAuthorLabel.Content = "Author";
                BookAuthorLabel.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                BookAuthorTextBox.Background = System.Windows.Media.Brushes.PaleVioletRed;
                BookAuthorLabel.Content = "Author can not be empty";
                BookAuthorLabel.Foreground = System.Windows.Media.Brushes.Red;
            }
            AddBookButton.IsEnabled = (ValidTitle() & ValidAuthor() & ValidISBN().result);
        }

        private bool ValidAuthor()
        {
            return !String.IsNullOrEmpty(BookAuthorTextBox.Text);
        }

        private void BookISBNTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (bool result, string errorMessage) = ValidISBN();
            if (!result)
            {
                BookISBNTextBox.Background = System.Windows.Media.Brushes.PaleVioletRed;
                ISBNLabel.Content = errorMessage;
                ISBNLabel.Foreground = System.Windows.Media.Brushes.Red;
            }
            else
            {
                BookISBNTextBox.Background = System.Windows.Media.Brushes.White;
                ISBNLabel.Content = "ISBN";
                ISBNLabel.Foreground = System.Windows.Media.Brushes.Black;
            }

            AddBookButton.IsEnabled = (ValidTitle() & ValidAuthor() & ValidISBN().result);
        }

        private (bool result, string errorMessage) ValidISBN()
        {
            string regexPattern = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";

            if (String.IsNullOrEmpty(BookISBNTextBox.Text))
            {
                return (false, "ISBN can not be empty");
            }
            else if (!Regex.IsMatch(BookISBNTextBox.Text, regexPattern))
            {
                return (false, "Not a valid ISBN number");
            }

            return (true, "");
        }

        private void BookListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = BookListBox.SelectedItem != null;
        }
    }
}
