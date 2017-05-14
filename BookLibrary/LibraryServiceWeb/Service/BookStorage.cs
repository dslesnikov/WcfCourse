using System.Collections.Generic;
using LibraryServiceWeb.Models;

namespace LibraryServiceWeb.Service
{
    public class BookStorage
    {
        public List<Book> BookList = new List<Book>
        {
            new Book
            {
                Id = 1,
                Name = "The C Programming Language",
                Author = "Kernighan, Ritchie",
                PublishYear = 1975,
                Type = BookType.Other
            },
            new Book
            {
                Id = 2,
                Name = "Homotopy Type Theory: Univalent Foundations of Mathematics",
                Author = "The Univalent Foundations Program",
                PublishYear = 2013,
                Type = BookType.Scientific
            },
            new Book
            {
                Id = 3,
                Name = "Certified Programming with Dependent Types",
                Author = "Adam Chlipala",
                PublishYear = 2013,
                Type = BookType.Scientific
            }
        };
    }
}