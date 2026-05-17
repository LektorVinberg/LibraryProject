using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra
{
    public class BookReservation
    {
        public Customer Customer { get; set; }
        public Book Book { get; set; }
        public DateTime ReservationDate { get; set; }
        public int QueuePlace { get; set; }

        public BookReservation(Customer customer, Book book, DateTime reservationDate, int queuePlace)
        {
            Customer = customer;
            Book = book;
            ReservationDate = reservationDate;
            QueuePlace = queuePlace;
        }
    }
}
