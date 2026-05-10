using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public String ISBN { get; set; }
        public BookState Status { get; set; } = BookState.Available;

        public Book(string title, string author, string isbn, BookState status)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Status = status;
        }

        internal string GetDetails()
        {
            return $"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Status: {Status}";
        }

        internal void SetStatus(BookState status)
        {
            Status = status;
        }

    }

 

    public enum BookState
    {
        Available,
        OnLoan
    }
}