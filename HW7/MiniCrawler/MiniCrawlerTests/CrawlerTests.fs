module MiniCrawlerTests

open System.IO
open NUnit.Framework
open FsUnit
open Crawler

let getTag (a:'a) = 
  let (uc,_) = Microsoft.FSharp.Reflection.FSharpValue.GetUnionFields(a, typeof<'a>)
  uc.Name

[<Test>]
let ``Fetch should work if url is correct`` () =
    fetchAsync "https://github.com/Alexander-Ploskin" |> Async.RunSynchronously |> getTag |> should equal "Some"

[<Test>]
let ``Fetch should do nothing if url is not correct`` () =
    fetchAsync "bad url" |> Async.RunSynchronously |> getTag |> should equal "None"

[<Test>]
let ``Save content should work if path is correct`` () =
    let path = "../../../TestDir/test.txt"
    let content = "text"
    saveContentAsync path content |> Async.RunSynchronously |> ignore
    File.ReadAllText(path) |> should equal content
    File.Delete(path)

let expectedLinks = [
    "https://docs.github.com/en/articles/blocking-a-user-from-your-personal-account";
    "https://docs.github.com/en/articles/reporting-abuse-or-spam";
    "https://docs.github.com/categories/setting-up-and-managing-your-github-profile"
]

[<Test>]
let ``Links finder should work correctly`` () =
    File.ReadAllText("../../../TestDir/github.html") |> getAllLinks |> should equal expectedLinks
