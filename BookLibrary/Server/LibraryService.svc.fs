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
            _bookStorage.Books
            |> List.tryFind (fun book -> book.Id = bookId )
            |> function None -> null | Some book -> book

        member x.GetBooks authorName =
            _bookStorage.Books
            |> List.filter (fun book -> book.Author = authorName)
            |> Seq.cast

        member x.TakeBook bookId =
            _bookStorage.Books 
                |> List.tryFind (fun book -> book.Id = bookId)
                |> function
                    | None -> failwith "Book was not found"
                    | Some book ->
                        match book.Taken with
                        | true -> failwith "Book is already taken"
                        | _ -> book.Taken <- true

        member x.ReturnBook bookId =
            _bookStorage.Books
                |> List.tryFind (fun book -> book.Id = bookId)
                |> function
                    | None -> failwith "Book was not found"
                    | Some book ->
                        match book.Taken with
                        | true -> book.Taken <- false
                        | _ -> failwith "Book was not taken"
