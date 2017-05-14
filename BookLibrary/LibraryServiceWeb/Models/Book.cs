using System;

namespace LibraryServiceWeb.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }
        public BookType Type { get; set; }
        public bool Taken { get; set; }
        public UserInfo TakerInfo { get; set; }
        public DateTime TakenDateTime { get; set; }
    }
}