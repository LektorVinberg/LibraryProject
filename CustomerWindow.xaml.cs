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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private LibraryLogic _libraryLogic;

        public CustomerWindow(LibraryLogic libraryLogic)
        {
            InitializeComponent();
            _libraryLogic = libraryLogic;
            InitializeCustomerList();
            UpdateCustomerCountLabel();
        }

        private void NameListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CloseWIndowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCustomerName())
            {
                _libraryLogic.AddCustomer(NewCustomerName.Text);
                InitializeCustomerList();
                UpdateCustomerCountLabel();
            }
        }

        private bool ValidateCustomerName()
        {
            if (NewCustomerName.Text.Length > 0)
            {
                NewCustomerName.Background = System.Windows.Media.Brushes.White;
                NewCustomerNameLabel.Content = "Name";
                NewCustomerNameLabel.Foreground = System.Windows.Media.Brushes.Black;
                return true;
            }
            NewCustomerName.Background = System.Windows.Media.Brushes.PaleVioletRed;
            NewCustomerNameLabel.Content = "Name cannot be empty";
            NewCustomerNameLabel.Foreground = System.Windows.Media.Brushes.Red;
            return false;
        }
        private void UpdateCustomerCountLabel()
        {
            CustomerCountLabel.Content = $"Customer count: {_libraryLogic.GetCustomers().Count}";
        }

        private void InitializeCustomerList()
        {
            CustomerListbox.ItemsSource = _libraryLogic.GetCustomers();
            CustomerListbox.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected customer from the listbox
            var selectedCustomer = CustomerListbox.SelectedItem as Customer;
            if (selectedCustomer != null)
            {
                // Remove the selected customer using the library logic
                _libraryLogic.RemoveCustomer(selectedCustomer.CustomerID);
                // Update the customer list and count label
                InitializeCustomerList();
                UpdateCustomerCountLabel();
            }
        }

        private void LoanButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = CustomerListbox.SelectedItem as Customer;
            if (selectedCustomer != null)
            {
                BookLoanWindow bookLoanWindow = new BookLoanWindow(_libraryLogic, selectedCustomer);
                bookLoanWindow.ShowDialog();
            }

        }

        private void CustomerListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCustomer = CustomerListbox.SelectedItem as Customer;
            if (selectedCustomer != null)
                if (selectedCustomer.LoanedBooks.Count > 0)
                {
                    LoanedBooksLabel.Content = $"Borrowed Books: {selectedCustomer.LoanedBooks.Count}";
                }
                else
                {
                    LoanedBooksLabel.Content = "Borrowed Books: None";
                }
        }

        private void NewCustomerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCustomerName();
        }
    }
}

