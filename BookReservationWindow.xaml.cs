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
    /// Interaction logic for BookReservationWindow.xaml
    /// </summary>
    public partial class BookReservationWindow : Window
    {
        private LibraryLogic _libraryLogic;
        public BookReservationWindow(LibraryLogic libraryLogic)
        {
            InitializeComponent();
            _libraryLogic = libraryLogic;
            InitializeListBoxes();

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void InitializeListBoxes()
        {
            CustomerListBox.ItemsSource = _libraryLogic.GetCustomers();
            CustomerListBox.Items.Refresh();
            BookListBox.ItemsSource = _libraryLogic.GetBooks();
            BookListBox.Items.Refresh();
            ReservedBooksListBox.ItemsSource = _libraryLogic.GetReservations();
            ReservedBooksListBox.Items.Refresh();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = CustomerListBox.SelectedItem as Customer;
            var book = BookListBox.SelectedItem as Book;
            if (customer == null || book == null)
            {
                MessageBox.Show("Please select a customer and a book to make reservation.");
                return;
            }
            var result = _libraryLogic.ReserveBook(book.ISBN, customer.CustomerID);
            MessageBox.Show(result);
            ReservedBooksListBox.Items.Refresh();
        }

        private void RemoveReservationButton_Click(object sender, RoutedEventArgs e)
        {
            var reservation = ReservedBooksListBox.SelectedItem as BookReservation;
            if (reservation == null)
            {
                MessageBox.Show("Please select a reservation to remove.");
                return;
            }
            var result = _libraryLogic.CancelReservation(reservation.Book.ISBN, reservation.Customer.CustomerID);
            MessageBox.Show(result);
            ReservedBooksListBox.Items.Refresh();
        }

        private void BorrowButton_Click(object sender, RoutedEventArgs e)
        {
            var reservation = ReservedBooksListBox.SelectedItem as BookReservation;
            if (reservation == null)
            {
                MessageBox.Show("Please select a reservation to borrow.");
                return;
            }
            if (reservation.QueuePlace > 1)
            {
                MessageBox.Show("You are not first in the reservation queue. Please wait for your turn.");
                return;
            }
            else
            {
                var result = _libraryLogic.LoanBook(reservation.Book.ISBN, reservation.Customer.CustomerID);
                MessageBox.Show(result);
                if (result == "Book loaned successfully.")
                {
                result = _libraryLogic.CancelReservation(reservation.Book.ISBN, reservation.Customer.CustomerID);
                MessageBox.Show(result);
                ReservedBooksListBox.Items.Refresh();
                }



            }
        }
    }
}
