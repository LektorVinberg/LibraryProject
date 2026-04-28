using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra
{
    internal class Book
    {
        string Title { get; set; }
        string Author { get; set; }
        String ISBN { get; set; }
        BookState Status { get; set; }

        internal Book(string title, string author, string isbn, BookState status)
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

    enum BookState
    {
        Available,
        OnLoan
    }
}
