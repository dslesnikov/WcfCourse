open System
open System.ServiceModel
open BookLibrary.Server

[<EntryPoint>]
let main argv =
    use host = new ServiceHost(typedefof<LibraryService>)
    host.Open()

    printfn "Service is running at %s" (host.Description.Endpoints.[0].Address.ToString())
    printfn "Press Enter to shut down service"
    Console.ReadLine() |> ignore
    host.Close() |> ignore
    0