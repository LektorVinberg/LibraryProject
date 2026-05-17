using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra
{
    public class Customer
    {
        private string name;
        public string Name { 
            get { return name; }
            set {
                if (!string.IsNullOrEmpty(value))
                {
                    name = value;
                }
            }
        }
        public Guid CustomerID { get; set; }
        public Dictionary<Guid, DateTime> LoanedBooks { get; set; }

        public Customer(string name)
        {
            Name = name;
            CustomerID = Guid.NewGuid();
            LoanedBooks = new Dictionary<Guid, DateTime>();
        }

        internal void AddLoan(Book book)
        {
            LoanedBooks.Add(book.Id, DateTime.Now);
        }

        internal void RemoveLoan(Guid bookId)
        {
            LoanedBooks.Remove(bookId);
        }

        internal string GetInfo()
        {
            return $"Name: {Name}, Customer ID: {CustomerID}, Loaned Books: {LoanedBooks.Count}";
        }

        
    }
}
