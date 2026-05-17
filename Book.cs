using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Libra
{
    public class Book
    {
        private string title;
        private string author;
        private string isbn;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    title = value;
                }
            }
        }
        public string Author
        {
            get { return author; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    author = value;
                }
            }

        }
        public string ISBN
        {
            get { return isbn; }
            set
            {
                string regexPattern = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";
                if (!string.IsNullOrEmpty(value) && Regex.IsMatch(value, regexPattern))
                {
                    isbn = value;
                }
            }
        }

        public Guid Id { get; }
        public BookState Status { get; set; } = BookState.Available;

        public Book(string title, string author, string isbn, BookState status)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Status = status;
            Id = Guid.NewGuid();
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