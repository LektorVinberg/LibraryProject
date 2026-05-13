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
    }
}
