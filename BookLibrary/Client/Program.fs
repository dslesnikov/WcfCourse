open System
open ClientProxy.LibraryServiceReference

[<EntryPoint>]
let main argv = 
    let mutable service = new LibraryServiceClient()
    service.EnterLibrary(0, "Jhon")
    let mutable book = service.GetBook 1
    printfn "Author of the book %s" book.Author
    printfn "Book %d is taken? - %b" 1 book.Taken
    service.TakeBook 1
    service.SaveChanges()

    service <- new LibraryServiceClient()
    service.EnterLibrary(0, "Jhon")
    book <- service.GetBook 1
    printfn "Book %d is taken? - %b" 1 book.Taken
    service.ReturnBook 1
    service.SaveChanges()

    service <- new LibraryServiceClient()
    service.EnterLibrary(0, "Jhon")
    book <- service.GetBook 1
    printfn "Book %d is taken? - %b" 1 book.Taken

    let books = service.GetBooks "The Univalent Foundations Program"
    books |> Array.iter (fun book -> printfn "Book name: %s" book.Name)

    let newBook =
        new Book(
            Name = "My cool book",
            Type = BookType.ArtisticLiterature,
            Author = "Me",
            Id = 98,
            PublishYear = 2017)
    service.AddBook newBook
    let myBooks = service.GetBooks "Me"
    myBooks |> Array.iter (fun book -> printfn "Book name: %s" book.Name)
    service.SaveChanges()
    0
