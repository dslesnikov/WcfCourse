using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Server
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

    [ServiceBehavior]
    public class LibraryService : ILibraryService
    {
        private const int MaxBooksInOneHands = 5;

        private static readonly BookStorage Storage = new BookStorage();

        public void AddBook(Book book)
        {
            Storage.BookList.Add(book);
        }

        public Book GetBook(string bookId)
        {
            var parsed = int.Parse(bookId);
            var book = Storage.BookList.FirstOrDefault(x => x.Id == parsed);
            if (book == null)
            {
                throw new FaultException("Книга не найдена");
            }
            return book;
        }

        public IEnumerable<Book> GetBooks(string authorName)
        {
            return Storage.BookList.Where(x => x.Author == authorName);
        }

        public void TakeBook(string userId, string userName, string bookId)
        {
            var parsedUserId = int.Parse(userId);
            var parsedBookId = int.Parse(bookId);
            var book = Storage.BookList.FirstOrDefault(x => x.Id == parsedBookId);
            if (book == null || book.Taken || Storage.BookList.Count(x => x.Taken && x.TakerInfo.Id == parsedUserId) >= MaxBooksInOneHands)
            {
                throw new Exception("Всё плохо");
            }
            book.Taken = true;
            book.TakenDateTime = DateTime.Now;
            book.TakerInfo = new UserInfo {Id = parsedUserId, Name = userName};
        }

        public void ReturnBook(string userId, string userName, string bookId)
        {
            var parsedUserId = int.Parse(userId);
            var parsedBookId = int.Parse(bookId);
            var book = Storage.BookList.FirstOrDefault(x => x.Id == parsedBookId);
            if (book == null || !book.Taken || book.TakerInfo.Id != parsedUserId)
            {
                throw new Exception("Всё плохо");
            }
            book.Taken = false;
            book.TakerInfo = null;
        }
    }
}
