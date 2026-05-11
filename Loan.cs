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
        private decimal lateFee = 0;

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
        private DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                if (DateTime.Now < value)
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
                return lateFee;
            }
            set { 
                if (value <= 0)
                {
                    lateFee = value;
                }
                else
                {
                    //TODO: console error message or GUI message?
                }
            }
        }

        public Loan(Customer customer, Book book, DateTime dueDate)
        {
            this.Customer = customer;
            this.Book = book;
            this.DueDate = dueDate;
        }
    }
}
