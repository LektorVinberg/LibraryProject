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

    public partial class BookLoanWindow : Window
    {
        private LibraryLogic _libraryLogic;
        private Customer _customer;

        public BookLoanWindow(LibraryLogic libraryLogic, Customer customer)
        {
            InitializeComponent();
            _libraryLogic = libraryLogic;
            _customer = customer;
            ActualCustomerLabel.Content = $"Borrowed books for {_customer.Name}";
            InitializeBookLists();

        }

        private void InitializeBookLists()
        {
            BorrowedBookListBox.ItemsSource = _libraryLogic.GetBorrowedBooks(_customer.CustomerID);
            BorrowedBookListBox.Items.Refresh();
            AvailableBooksListBox.ItemsSource = _libraryLogic.GetAvailableBooks();
            AvailableBooksListBox.Items.Refresh();  
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void BorrowBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableBooksListBox.SelectedItem is Book selectedBook)
            {
                string result = _libraryLogic.LoanBook(selectedBook.ISBN, _customer.CustomerID);
                MessageBox.Show(result);
                InitializeBookLists();
            }
        }

        private void ReturnBookButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (BorrowedBookListBox.SelectedItem is Book selectedBook)
            {
                string result = _libraryLogic.ReturnBook(selectedBook.ISBN, _customer.CustomerID);
                MessageBox.Show(result);
                InitializeBookLists();
            }
        }
    }
}
