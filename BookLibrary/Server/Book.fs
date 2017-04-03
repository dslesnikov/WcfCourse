namespace BookLibrary.Server

open System;
open System.Runtime.Serialization
open System.ServiceModel

type public BookType = 
    | Scientific = 0
    | Magazine = 1
    | ArtisticLiterature = 2
    | Other = 3

[<AllowNullLiteral>]
[<DataContract(Namespace=
    "http://www.scottseely.com/WCFDemo")>]
type public Book() =
    let mutable _id : int = 0
    let mutable _name : string = String.Empty
    let mutable _authorName : string = String.Empty
    let mutable _publishedYear : int = 0
    let mutable _bookType : BookType = BookType.Scientific

    [<DataMember>]
    member public book.Id
        with get() = _id
        and set(value) = _id <- value

    [<DataMember>]
    member public book.Name
        with get() = _name
        and set(value) = _name <- value
    
    [<DataMember>]
    member public book.Author
        with get() = _authorName
        and set(value) = _authorName <- value

    [<DataMember>]
    member public book.PublishYear
        with get() = _publishedYear
        and set(value) = _publishedYear <- value

    [<DataMember>]
    member public book.Type
        with get() = _bookType
        and set(value) = _bookType <- value
