module Crawler

open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Threading


let linkRegex = 
    Regex("<a href\s*=\s*\"?(https?://[^\"]+)\"?\s*>", RegexOptions.Compiled)

let getAllLinkedPages (html : string) =
    [for matches in (linkRegex.Matches(html) : MatchCollection) -> matches.Groups.[1].Value]

let fetchAsync (url : string) =
       async {
           try
               let request = WebRequest.Create(url)
               use! response = request.AsyncGetResponse()
               use stream = response.GetResponseStream()
               use reader = new StreamReader(stream)
               let html = reader.ReadToEnd()
               do printfn "[.NET Thread %d]" Thread.CurrentThread.ManagedThreadId
               do printfn "Downloaded page %s with size of %d" url html.Length
               return Some html
           with 
               | _ -> return None
       }

let saveContentAsync (path : string) (content : string) =
    async {
        try
            use stream = File.Create(path)
            use writer = new StreamWriter(stream)
            do printfn "[.NET Thread %d]" Thread.CurrentThread.ManagedThreadId
            return writer.Write(content)
        with
        | :?IOException -> failwith "Invalid path"
    }

let savePageAsync (path : string) (content : Option<_>) =
    match content with
    | Some html -> saveContentAsync path html
    | _ -> async { return() }

let downloadLinkedPages (url : string) (path : string) =
    let mainPageContent = fetchAsync url |> Async.RunSynchronously
    match mainPageContent with
    | Some content -> Directory.CreateDirectory(path) |> ignore
                      let links = content |> getAllLinkedPages
                      let downloaded = links |> List.map (fun link -> link |> fetchAsync) |> Async.Parallel |> Async.RunSynchronously
                      downloaded |> Array.mapi (fun i option -> savePageAsync $"{path}/{i}.html" option) |> Async.Parallel |> Async.RunSynchronously |> ignore
    | None -> ()

downloadLinkedPages "https://github.com/Alexander-Ploskin" "GitHub"
