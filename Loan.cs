using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra
{
    public class Loan
    {
        private Customer customer;      //this attribute might be unneccessary since Loan objects should be in a list of the customer class
        private Book book;
        private DateTime dueDate;
        private decimal lateFee = 0;    //might remove this attribute and just calculate the fee when prompted instead

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
            Customer = customer;
            Book = book;
            DueDate = dueDate;
        }

        public decimal GetLateFee()
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
}
