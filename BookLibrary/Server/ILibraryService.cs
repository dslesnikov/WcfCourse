using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Server
{
    [ServiceContract]
    public interface ISaveChangesOperationCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnSaveChanges();
    }

    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/add", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void AddBook(Book book);

        [OperationContract]
        [WebGet(UriTemplate = "/book/{bookId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Book GetBook(string bookId);

        [OperationContract]
        [WebGet(UriTemplate = "/books/{authorName}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<Book> GetBooks(string authorName);

        [OperationContract]
        [WebGet(UriTemplate = "/take/{bookId}/{userId}/{userName}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void TakeBook(string userId, string userName, string bookId);

        [OperationContract]
        [WebGet(UriTemplate = "/return/{bookId}/{userId}/{userName}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void ReturnBook(string userId, string userName, string bookId);
    }
}