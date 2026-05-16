using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace Libra
{

    public class LibraryLogic
    {
        private List<Book> books;
        private List<Customer> customers;
        private const decimal LATE_FEE_PER_DAY = 10;
        private const double DAYS_UNTIL_DUE = 5;
        public record OverDue(string Title, string Name, decimal LateFee);

        internal LibraryLogic()
        {
            books = new List<Book>();
            customers = new List<Customer>();
        }
        internal List<Book> GetBooks()
        {
            return books;
        }

        internal List<Book> GetAvailableBooks()
        {
            return books.Where(b => b.Status == BookState.Available).ToList();
        }

        internal List<Book> GetBorrowedBooks(Guid customerId)
        {
            var customer = customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer != null)
            {
                return customer.LoanedBooks.Keys.ToList();
            }
            return new List<Book>();
        }

        // Add a book to the book list, make sure that it is a new book 
        internal void AddBook(Book book)
        {
            if (books.Find(b => b.ISBN == book.ISBN) == null)
            {
                books.Add(book);

            }
        }

        internal void RemoveBook(Book book)
        {
            int bookIndex = books.FindIndex(b => b.ISBN == book.ISBN);
            if (bookIndex != -1)
            {
                books.RemoveAt(bookIndex);
            }
        }

        internal int GetBookCount()
        {
            return books.Count;
        }

        internal void AddCustomer(string customerName)
        {
            Customer customer = new Customer(customerName);
            customers.Add(customer);
        }

        internal List<Customer> GetCustomers()
        {
            return customers;
        }

        internal void RemoveCustomer(Guid customerId)
        {
            int customerIndex = customers.FindIndex(c => c.CustomerID == customerId);
            if (customerIndex != -1)
            {
                customers.RemoveAt(customerIndex);
            }
        }

        internal string LoanBook(string isbn, Guid customerId)
        {
            var book = books.FirstOrDefault(b => b.ISBN == isbn);
            var customer = customers.FirstOrDefault(c => c.CustomerID == customerId);

            if (book == null) return "Book not found.";
            if (customer == null) return "Customer not found.";
            if (book.Status != BookState.Available) return "Book is not available.";

            book.SetStatus(BookState.OnLoan);
            customer.AddLoan(book);
            return "Book loaned successfully.";
        }


        internal string ReturnBook(string isbn, Guid customerId)
        {

            var book = books.FirstOrDefault(b => b.ISBN == isbn);
            var customer = customers.FirstOrDefault(c => c.CustomerID == customerId);

            if (book == null) return "Book not found";
            if (customer == null) return "Customer not found.";

            bool loan = customer.LoanedBooks.ContainsKey(book);

            if (loan)
            {
                customer.LoanedBooks.Remove(book);
                book.SetStatus(BookState.Available);
                return "Book returned successfully.";
            }

            return "This book was not loaned to this customer.";


            //var book = books.FirstOrDefault(b => b.ISBN == isbn);
            //var customer = customers.FirstOrDefault(c => c.CustomerID == customerId);
            //if (book == null) return "Book not found.";
            //if (customer == null) return "Customer not found.";
            //// Check ISBN because the list is a copy when you deserialize
            //if (!customer.LoanedBooks.Any(b => b.ISBN == isbn)) return "This book was not loaned to this customer.";
            //    book.SetStatus(BookState.Available);
            //customer.RemoveLoan(book);
        }

        public List<OverDue> FindOverDue()
        {
            List<OverDue> overDueList = new List<OverDue>();

            foreach (Customer customer in customers)
            {
                Dictionary<Book, DateTime> loanedBooks = customer.LoanedBooks;

                foreach (Book book in loanedBooks.Keys)
                {
                    decimal lateFee = 0;
                    DateTime dueDate = loanedBooks[book].AddSeconds(DAYS_UNTIL_DUE);    //AddSeconds() is called instead of AddDays(). This is for demonstration purposes only.
                    if (dueDate <= DateTime.Now)
                    {

                        lateFee = CalculateOverDueFee(dueDate);

                        overDueList.Add(
                                new OverDue(book.Title, customer.Name, lateFee)
                            );
                    }
                }
            }
            return overDueList;
        }

        private decimal CalculateOverDueFee(DateTime dueDate)
        {
            TimeSpan timeSpan = DateTime.Now - dueDate;
            return timeSpan.Seconds * LATE_FEE_PER_DAY;
        }

        internal void BackupLists()
        {
            // Implement backup logic here, e.g., save books and customers to a file or database
            if (customers.Count > 0)
            {
                string jsonString = JsonSerializer.Serialize(customers);
                File.WriteAllText(@"..\..\..\Data\customers.Json", jsonString);
            }
            if (books.Count > 0)
            {
                string jsonString = JsonSerializer.Serialize(books);
                File.WriteAllText(@"..\..\..\Data\books.Json", jsonString);
            }
        }

        internal void RetrieveLists()
        {
            // Implement restore logic here, e.g., load books and customers from a file or database
            if (File.Exists(@"..\..\..\Data\customers.Json"))
            {
                string jsonString = File.ReadAllText(@"..\..\..\Data\customers.Json");
                customers = JsonSerializer.Deserialize<List<Customer>>(jsonString);
            }
            if (File.Exists(@"..\..\..\Data\books.Json"))
            {
                string jsonString = File.ReadAllText(@"..\..\..\Data\books.Json");
                books = JsonSerializer.Deserialize<List<Book>>(jsonString);
            }
        }

        internal List<Book> SearchBooks(string query, string mode)
        {
            List<Book> result = new List<Book>();

            if (string.IsNullOrWhiteSpace(query))
                return books.ToList();

            query = query.Trim();

            while (query.Contains("  "))
            {
                query = query.Replace("  ", " ");
            }

            foreach (Book b in books)
            {
                bool match = false;

                if (mode == "Title" && b.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                    match = true;

                if (mode == "Author" && b.Author.Contains(query, StringComparison.OrdinalIgnoreCase))
                    match = true;

                if (mode == "ISBN" && b.ISBN.Contains(query))
                    match = true;

                if (mode == "AllFields" && (
                    b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    b.ISBN.Contains(query)))
                    match = true;

                if (match)
                    result.Add(b);
            }

            return result;
        }

        // Generate a report showing borrowed books
        // and the customers who borrowed them
        internal List<string> GetLoanReport()
        {
            List<string> report = new List<string>();

            foreach (Customer customer in customers)
            {
                foreach (var loan in customer.LoanedBooks)
                {
                    Book book = loan.Key;
                    DateTime loanDate = loan.Value;

                    report.Add(
                        $"Title: {book.Title} | " +
                        $"Customer: {customer.Name} | " +
                        $"Loan Date: {loanDate}"
                    );
                }
            }

            return report;
        }
    }
}
