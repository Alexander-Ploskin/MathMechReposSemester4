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

let voidAsync =
    async {
        return ()
    }

let savePageAsync (path : string) (content : Option<_>) =
    match content with
    | Some html -> saveContentAsync path html
    | _ -> voidAsync

let sites = ["https://github.com/Alexander-Ploskin"; "http://se.math.spbu.ru"; "https://habr.com/ru/company/microsoft/blog/335560/"]
let options = sites |> List.map (fun site -> site |> fetchAsync) |> Async.Parallel |> Async.RunSynchronously

options |> Array.mapi (fun i option -> savePageAsync $"{i}.html" option) |> Async.Parallel |> Async.RunSynchronously |> ignore
