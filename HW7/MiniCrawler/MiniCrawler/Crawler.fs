module Crawler

open System.IO
open System.Net
open System.Text.RegularExpressions

/// Finds all links matching with linkRegex
let getAllLinks (html : string) =
    [for matches in (Regex("<a href\s*=\s*\"?(https?://[^\"]+)\"?\s*>", RegexOptions.Compiled).Matches(html) : MatchCollection) 
    -> matches.Groups.[1].Value]

/// Downloads page by url asynchronously
let fetchAsync (url : string) =
       async {
           try
               let request = WebRequest.Create(url)
               use! response = request.AsyncGetResponse()
               use stream = response.GetResponseStream()
               use reader = new StreamReader(stream)
               let html = reader.ReadToEnd()
               return Some html
           with 
               | _ -> printfn "Can't reach the page"
                      return None
       }

/// Prints result of downloading
let printResult url (downloaded : Option<string>) = 
    printfn "Pages linked to this page"
    match downloaded with
    | Some html -> printfn "%s    Length: %d symbols" url html.Length
    | _ -> printfn "Bad response for %s" url

/// Downloads all pages into the path which linked in the page by the url
let downloadLinkedPages (url : string) =
    let mainPageContent = fetchAsync url |> Async.RunSynchronously
    match mainPageContent with
    | Some content -> let links = content |> getAllLinks
                      let downloaded = links |> List.map (fun link -> link |> fetchAsync) |> Async.Parallel |> Async.RunSynchronously
                      downloaded |>  Array.iteri (fun i (result : string option) -> printResult (links.Item i) result)
                      
    | None -> ()

// Example of usage
downloadLinkedPages "https://github.com/Alexander-Ploskin"
