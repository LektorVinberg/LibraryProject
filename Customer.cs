using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra
{
    public class Customer
    {
        public string Name { get; set; }
        public Guid CustomerID { get; set; }
        public List<Book> LoanedBooks { get; set; } = new List<Book>();

        public Customer(string name)
        {
            Name = name;
            CustomerID = Guid.NewGuid();
            LoanedBooks = new List<Book>();
        }

        internal void AddLoan(Book book)
        {
            LoanedBooks.Add(book);
        }

        internal void RemoveLoan(Book book)         {
            LoanedBooks.Remove(book);
        }

        internal string GetInfo()
        {
            return $"Name: {Name}, Customer ID: {CustomerID}, Loaned Books: {LoanedBooks.Count}";
        }

        
    }
}
