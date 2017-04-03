namespace BookLibrary.Server

open System.Runtime.Serialization
open System.ServiceModel


[<ServiceContract(Namespace=
    "http://www.scottseely.com/WCFDemo")>]
type ILibraryService =
    [<OperationContract>]
    abstract AddBook: book:Book -> unit

    [<OperationContract>]
    abstract GetBook: bookId:int -> Book

    [<OperationContract>]
    abstract GetBooks: bookAuthor:string -> seq<Book>

    [<OperationContract>]
    abstract TakeBook: bookId:int -> unit

    [<OperationContract>]
    abstract ReturnBook: bookId:int -> unit
