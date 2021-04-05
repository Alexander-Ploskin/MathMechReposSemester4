module Crawler

open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Threading

// Regex which matches with links in the <a href="http://..."> form 
let linkRegex = 
    Regex("<a href\s*=\s*\"?(https?://[^\"]+)\"?\s*>", RegexOptions.Compiled)

// Finds all links matching with linkRegex
let getAllLinks (html : string) =
    [for matches in (linkRegex.Matches(html) : MatchCollection) -> matches.Groups.[1].Value]

// Downloads page by url asynchronously
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
               | _ -> printfn "Can't reach the page"
                      return None
       }

// Saves content into new file by path asynchronously
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

// Gets function of saving page getten in a form of option asynchronously
let savePageAsync (path : string) (content : Option<_>) =
    match content with
    | Some html -> saveContentAsync path html
    | _ -> async { return() }

// Downloads all pages into the path which linked in the page by the url
let downloadLinkedPages (url : string) (path : string) =
    let mainPageContent = fetchAsync url |> Async.RunSynchronously
    match mainPageContent with
    | Some content -> Directory.CreateDirectory(path) |> ignore
                      let links = content |> getAllLinks
                      let downloaded = links |> List.map (fun link -> link |> fetchAsync) |> Async.Parallel |> Async.RunSynchronously
                      downloaded |> Array.mapi (fun i option -> savePageAsync $"{path}/{i}.html" option) |> Async.Parallel |> Async.RunSynchronously |> ignore
    | None -> ()

downloadLinkedPages "https://github.com/Alexander-Ploskin" "GitHub"
