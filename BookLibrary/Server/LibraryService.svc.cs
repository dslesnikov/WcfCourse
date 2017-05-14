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

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class LibraryService : ILibraryService
    {
        private const int MaxBooksInOneHands = 5;

        private static readonly BookStorage Storage = new BookStorage();
        private UserInfo _currentUser;
        private readonly List<int> _returnedBooks = new List<int>();
        private readonly List<int> _takenBooks = new List<int>();
        private ISaveChangesOperationCallback Callback => OperationContext.Current.GetCallbackChannel<ISaveChangesOperationCallback>();

        public void AddBook(Book book)
        {
            Storage.BookList.Add(book);
        }

        public void EnterLibrary(int userId, string userName)
        {
            _currentUser = new UserInfo
            {
                Id = userId,
                Name = userName
            };
        }

        public Book GetBook(int bookId)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == bookId);
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

        public void Leave() { }

        public void ReturnBook(int bookId)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == bookId);
            if (book == null)
            {
                throw new FaultException("Книга не найдена");
            }
            if (!book.Taken)
            {
                throw new FaultException("Книга не была ранее взята из библиотеки");
            }
            if (book.TakerInfo.Id != _currentUser.Id)
            {
                throw new FaultException("Не ты брал эту книгу из библиотеки");
            }
            _returnedBooks.Add(bookId);
        }

        public void SaveChanges()
        {
            foreach (var book in Storage.BookList.Where(x => _returnedBooks.Contains(x.Id)))
            {
                book.Taken = false;
                book.TakerInfo = null;
            }
            foreach (var book in Storage.BookList.Where(x => _takenBooks.Contains(x.Id)))
            {
                book.TakenDateTime = DateTime.Now;
                book.Taken = true;
                book.TakerInfo = _currentUser;
            }
            //if (Storage.BookList.Any(x => x.Taken && x.TakerInfo.Id == _currentUser.Id
            //    && (DateTime.Now - x.TakenDateTime) > TimeSpan.FromDays(30)))
            {
                Callback.OnSaveChanges();
            }
        }

        public void TakeBook(int bookId)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == bookId);
            if (book == null)
            {
                throw new FaultException("Книга не найдена");
            }
            if (book.Taken)
            {
                throw new FaultException("Эту книгу уже кто-то взял");
            }
            if (Storage.BookList.Count(x => x.Taken && x.TakerInfo.Id == _currentUser.Id) >= MaxBooksInOneHands)
            {
                throw new FaultException("У тебя и так дофига книг");
            }
            _takenBooks.Add(bookId);
        }
    }
}
