using System;
using System.Linq;
using LibraryServiceWeb.Models;
using LibraryServiceWeb.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryServiceWeb.Controllers
{
    [ApiVersion("0.9")]
    [Route("api/v{version:apiVersion}/library")]
    public class LibraryController : Controller
    {
        protected static readonly BookStorage Storage = new BookStorage();

        [HttpPost]
        public IActionResult Add([FromBody] Book newBook)
        {
            Storage.BookList.Add(newBook);
            return Ok();
        }

        [HttpGet("get/{id}")]
        public IActionResult Get(int id)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("getall/{authorName}")]
        public IActionResult GetBooks(string authorName)
        {
            var books = Storage.BookList.Where(x => x.Author == authorName);
            if (!books.Any())
            {
                return NotFound();
            }
            return Ok(books);
        }

        [HttpGet("take/{id}"), MapToApiVersion("0.9")]
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

        [HttpGet("return/{id}")]
        public virtual IActionResult Return(UserInfo userInfo, int id)
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

    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/library/")]
    public class LibraryControllerNew : LibraryController
    {
        [HttpGet("take/{id}/{userId}")]
        public IActionResult Take(int id, int userId)
        {
            var userInfo = new UserInfo {Id = userId};
            return Take(userInfo, id);
        }

        [HttpGet("return/{id}"), MapToApiVersion("2.0")]
        public IActionResult Return(int id)
        {
            var book = Storage.BookList.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            book.Taken = false;
            book.TakerInfo = null;
            return Ok();
        }
    }
}
