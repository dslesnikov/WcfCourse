using System.Collections.Generic;
using System.ServiceModel;

namespace Server
{
    [ServiceContract]
    public interface ISaveChangesOperationCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnSaveChanges();
    }

    [ServiceContract(CallbackContract = typeof(ISaveChangesOperationCallback), SessionMode = SessionMode.Required)]
    public interface ILibraryService
    {
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        void EnterLibrary(int userId, string userName);

        [OperationContract(IsInitiating = false)]
        void AddBook(Book book);

        [OperationContract(IsInitiating = false)]
        Book GetBook(int bookId);

        [OperationContract(IsInitiating = false)]
        IEnumerable<Book> GetBooks(string authorName);

        [OperationContract(IsInitiating = false)]
        void TakeBook(int bookId);

        [OperationContract(IsInitiating = false)]
        void ReturnBook(int bookId);

        [OperationContract(IsInitiating = false, IsOneWay = true)]
        void SaveChanges();

        [OperationContract(IsInitiating = false, IsTerminating = true, IsOneWay = true)]
        void Leave();
    }
}