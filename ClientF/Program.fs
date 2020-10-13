open System
open System.Net
open System.Net.Http

type AsyncMaybeBuilder () =
        member this.Bind(x, f) =
            async {
                let! _x = x
                match _x with
                | Some s -> return! f s
                | None -> return None
            }

        member this.Return(x:'a option) =
            async{return x}
            
module Calculator =
    let async_maybe = new AsyncMaybeBuilder()
    
    let MakeRequestUrl val1 operator val2 =
        "http://localhost:5000/calculate?val1=" + val1 + 
        "&operation=" + operator +
        "&val2=" + val2
        
    let CheckStatusCode (response:HttpResponseMessage) =
        if response.StatusCode = HttpStatusCode.OK
        then response.Content.ReadAsStringAsync() |> Async.AwaitTask |> Some
        else None
        
    let GetResponse (url:string) =
        async{
                let httpClient = new HttpClient()
                let! response = httpClient.GetAsync(url) |> Async.AwaitTask
                let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                return content |> Some
        }
        
    let GetResult val1 operator val2 =
        async_maybe{
            let requestUrl = MakeRequestUrl val1 operator val2
            let! response = GetResponse requestUrl
            return response |> Some
        } |> Async.RunSynchronously

    let Calculate val1 operator val2 =
        let result = GetResult val1 operator val2
        match result with
        | Some result -> result
        | None -> "Error"
        

module Program =
    open Calculator

    [<EntryPoint>]
    let main argv =
        let val1 = Console.ReadLine()
        let operator = Console.ReadLine().Replace("+", "%2B")
        let val2 = Console.ReadLine()
        Console.WriteLine(Calculate val1 operator val2)
        0 // return an integer exit code
