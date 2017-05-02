namespace BookLibrary.Server

open System
open System.Collections.Generic
open System.ServiceModel

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

[<ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)>]
type LibraryService() =
    static let _bookStorage : BookStorage = new BookStorage()
    let mutable _currentUser : Option<UserInfo> = Option.None
    let mutable _takenBooks : list<int> = list.Empty
    let mutable _returnedBooks : list<int> = list.Empty

    interface ILibraryService with
        member x.EnterLibrary id name =
            _currentUser <- Option.Some(new UserInfo(Id = id, Name = name))

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
                        | _ ->
                            match _takenBooks.Length with
                            | 5 -> failwith "You are too greedy asshole"
                            | _ -> _takenBooks <- (bookId :: _takenBooks)

        member x.ReturnBook bookId =
            _bookStorage.Books
                |> List.tryFind (fun book -> book.Id = bookId)
                |> function
                    | None -> failwith "Book was not found"
                    | Some book ->
                        match book.Taken, book.TakerInfo with
                        | true, _currentUser -> _returnedBooks <- (bookId :: _returnedBooks)
                        | _ -> failwith "Book was not taken by you"

        member x.SaveChanges () =
            match _currentUser with
                | Option.None -> failwith "You didn't authenticated"
                | Some userInfo ->
                    List.iter (fun bookId  ->
                        _bookStorage.Books
                            |> List.tryFind (fun b -> b.Id = bookId)
                            |> function
                                | None -> failwith "Book was not found"
                                | Some book -> book.Taken <- true
                                               book.TakerInfo <- Some userInfo) _takenBooks
                    List.iter (fun bookId  ->
                        _bookStorage.Books
                            |> List.tryFind (fun b -> b.Id = bookId)
                            |> function
                                | None -> failwith "Book was not found"
                                | Some book -> book.Taken <- false
                                               book.TakerInfo <- None) _returnedBooks

        member x.Leave () = ()