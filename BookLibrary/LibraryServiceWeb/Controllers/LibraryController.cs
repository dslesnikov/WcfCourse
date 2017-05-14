using System;
using System.Linq;
using LibraryServiceWeb.Models;
using LibraryServiceWeb.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryServiceWeb.Controllers
{
    public class LibraryController : Controller
    {
        private static readonly BookStorage Storage = new BookStorage();

        [HttpPost]
        public IActionResult Add([FromBody] Book newBook)
        {
            Storage.BookList.Add(newBook);
            return Ok();
        }

        public IActionResult Get(int id)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        public IActionResult GetBooks(string authorName)
        {
            var books = Storage.BookList.Where(x => x.Author == authorName);
            if (!books.Any())
            {
                return NotFound();
            }
            return Ok(books);
        }

        public IActionResult Take(UserInfo userInfo, int id)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            if (book.Taken || Storage.BookList.Count(x => x.Taken && x.TakerInfo.Id == userInfo.Id) > 5)
            {
                return BadRequest();
            }
            book.Taken = true;
            book.TakerInfo = userInfo;
            book.TakenDateTime = DateTime.Now;
            return Ok(book);
        }

        public IActionResult Return(UserInfo userInfo, int id)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            if (!book.Taken || book.TakerInfo.Id != userInfo.Id)
            {
                return BadRequest();
            }
            book.Taken = false;
            book.TakerInfo = null;
            return Ok();
        }
    }
}
