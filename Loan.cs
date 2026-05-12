using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra
{
    public class Loan
    {
        private Customer customer;
        private Book book;
        private DateTime dueDate;

        public Customer Customer
        {
            get { return customer; }
            set
            {
                //TODO: needs some validation maybe?
                customer = value;
            }
        }
        public Book Book
        {
            get { return book; }
            set { book = value; }
        }
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                if (DateTime.Now.Day < value.Day)
                {
                    dueDate = value;
                }
                else
                {
                    //TODO: console error message or GUI message?
                }
            }
        }
        public decimal LateFee
        {
            get
            {
                if (dueDate < DateTime.Now)
                {
                    TimeSpan timeSpan = DateTime.Now - dueDate;
                    return timeSpan.Days * Globals.LATE_FEE_PER_DAY;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Loan(Customer customer, Book book, DateTime dueDate)
        {
            Customer = customer;
            Book = book;
            DueDate = dueDate;
        }
    }
}
