open System
open ClientProxy.LibraryServiceReference

[<EntryPoint>]
let main argv = 
    use service = new LibraryServiceClient()
    let book = service.GetBook 1
    printf "Author of the book %s" book.Author
    0 // return an integer exit code
