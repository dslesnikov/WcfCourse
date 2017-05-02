namespace BookLibrary.Server

open System;
open System.Runtime.Serialization
open System.ServiceModel

type public BookType = 
    | Scientific = 0
    | Magazine = 1
    | ArtisticLiterature = 2
    | Other = 3

type public UserInfo() =
    let mutable _id : int = 0
    let mutable _name : string = String.Empty;

    member public user.Id
        with get() = _id
        and set(value) = _id <- value

    member public user.Name
        with get() = _name
        and set(value) = _name <- value

[<AllowNullLiteral>]
[<DataContract>]
type public Book() =
    let mutable _id : int = 0
    let mutable _name : string = String.Empty
    let mutable _authorName : string = String.Empty
    let mutable _publishedYear : int = 0
    let mutable _bookType : BookType = BookType.Scientific
    let mutable _taken : bool = false
    let mutable _takerInfo : Option<UserInfo> = Option.None

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

    [<DataMember>]
    member public book.Taken
        with get() = _taken
        and set(value) = _taken <- value

    member public book.TakerInfo
        with get() = _takerInfo
        and set(value) = _takerInfo <- value
