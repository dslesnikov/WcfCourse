namespace BookLibrary.Server

open System
open System.Collections.Generic

type BookStorage() =
    let mutable _books : list<Book> = [
        new Book(
            Id = 1,
            Name = "The C Programming Language",
            Author = "Kernighan, Ritchie",
            PublishYear = 1975,
            Type = BookType.Other);
        new Book(
            Id = 2,
            Name = "Homotopy Type Theory: Univalent Foundations of Mathematics",
            Author = "The Univalent Foundations Program",
            PublishYear = 2013,
            Type = BookType.Scientific);
        new Book(
            Id = 3,
            Name = "Certified Programming with Dependent Types",
            Author = "Adam Chlipala",
            PublishYear = 2013,
            Type = BookType.Scientific);
        ]

    member storage.Books
        with get() = _books
        and set(value) = _books <- value

type LibraryService() =
    static let _bookStorage : BookStorage = new BookStorage()

    interface ILibraryService with
        member x.AddBook book =
            _bookStorage.Books <- (book::_bookStorage.Books)

        member x.GetBook bookId =
            let rec findBook (bookId:int) (bookList:list<Book>) : Book =
                match bookList with
                | [] -> null
                | (bookHead::bookTail) ->
                    let id = bookHead.Id
                    match id with
                    | x when x = bookId -> bookHead
                    | _ -> findBook bookId bookTail
            findBook bookId _bookStorage.Books

        member x.GetBooks authorName =
            let rec findBook (authorName:string) (bookList:list<Book>) (aggregator:list<Book>) : list<Book> =
                match bookList with
                | [] -> aggregator
                | (bookHead::bookTail) ->
                    let currentAuthorName = bookHead.Author
                    match currentAuthorName with
                    | x when x = authorName ->
                        findBook authorName bookTail (bookHead::aggregator)
                    | _ -> findBook authorName bookTail aggregator
            Seq.cast (findBook authorName _bookStorage.Books [])

        member x.TakeBook bookId = ()
        member x.ReturnBook bookId = ()