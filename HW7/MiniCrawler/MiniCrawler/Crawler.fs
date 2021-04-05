module Crawler

open System.IO
open System.Net
open System.Text.RegularExpressions

(*let downloadPageAsync (url : string) (path : string) =
    async {
        try
            let request = WebRequest.Create(url)
            use! response = request.AsyncGetResponse()
            use stream = response.GetResponseStream()
            use reader = new StreamReader(stream)
            use fileStream = File.Create(path)
            use writer = new StreamWriter(fileStream)
            let content = reader.ReadToEnd()
            do! writer.WriteAsync(content)
            return Some <| content
        with
        | _ -> return None
    }

let saveContentAsync (path : string) (content : string) =
    async {
        use stream = File.Create(path)
        use writer = new StreamWriter(stream)
        return writer.Write(content)
    }

let linkRegex = 
    Regex("<a href\s*=\s*\"?(https?://[^\"]+)\"?\s*>", RegexOptions.Compiled)

let getAllLinkedPages (html : string) =
    [for matches in (linkRegex.Matches(html) : MatchCollection) -> matches.Groups.[1].Value]

let downloadAllLinkedPages url  =
    let mainPage = downloadPageAsync url |> Async.RunSynchronously
    match mainPage with
    | Some html -> getAllLinkedPages html
                   |> List.map (fun link -> downloadPageAsync link)
                   |> Async.Parallel |> Async.RunSynchronously |> Array.toList
                   |> List.mapi (fun i page -> match page with
                                               | Some(content) -> saveContentAsync $"{i}.html" content
                                               | _ -> )
    | None -> printfn "Can't reach specified page"
              []

let pages = downloadAllLinkedPages "https://github.com/Alexander-Ploskin"
let contents = pages |> List.map (fun page -> match page with
                                               | Some(content) -> content
                                               | _ -> Async<>()) 
let tasks = contents |> List.mapi (fun i content -> saveContentAsync $"{i}.html" content)
tasks |> Async.Parallel |> Async.RunSynchronously |> ignore
*)



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
               | _ -> return None
       }

printfn "%A" (fetchAsync "https://github.com/Alexander-Ploskin" |> Async.RunSynchronously)
