namespace BookLibrary.Server

open System.Runtime.Serialization
open System.ServiceModel


[<ServiceContract(SessionMode = SessionMode.Required)>]
type ILibraryService =
    [<OperationContract(IsInitiating = true, IsTerminating = false)>]
    abstract EnterLibrary: userId:int -> userName:string -> unit

    [<OperationContract(IsInitiating = false)>]
    abstract AddBook: book:Book -> unit

    [<OperationContract(IsInitiating = false)>]
    abstract GetBook: bookId:int -> Book

    [<OperationContract(IsInitiating = false)>]
    abstract GetBooks: bookAuthor:string -> seq<Book>

    [<OperationContract(IsInitiating = false)>]
    abstract TakeBook: bookId:int -> unit

    [<OperationContract(IsInitiating = false)>]
    abstract ReturnBook: bookId:int -> unit

    [<OperationContract(IsInitiating = false, IsTerminating = true)>]
    abstract SaveChanges: unit -> unit

    [<OperationContract(IsTerminating = true)>]
    abstract Leave: unit -> unit
