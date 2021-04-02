module Test1Tests

open FsUnit
open NUnit.Framework
open SquarePrinter

[<Test>]
let ``Should return none if length is negative``() =
    stringSquare -1 |> should equal None

[<Test>]
let ``Should return empty if length is zero`` () =
    stringSquare 0 |> should equal <| Some("\n")

[<Test>]
let ``Should be correct if length is 1`` () =
    stringSquare 1 |> should equal <| Some("*\n")

[<Test>]
let ``Should work correctly if length is big`` () =
    stringSquare 3 |> should equal <| Some ("***\n* *\n***\n")
