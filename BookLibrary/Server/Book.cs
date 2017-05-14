using System;
using System.Runtime.Serialization;

namespace Server
{
    [DataContract]
    public class Book
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public int PublishYear { get; set; }
        [DataMember]
        public BookType Type { get; set; }
        [DataMember]
        public bool Taken { get; set; }
        public UserInfo TakerInfo { get; set; }
        public DateTime TakenDateTime { get; set; }
    }
}