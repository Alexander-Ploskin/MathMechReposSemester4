module RoundingCounterTests

open FsUnit
open NUnit.Framework
open RoundingWorkflow
open System

[<Test>]
let ``Should fail if accuracy is negative``() =
    (fun () -> RoundingCounter(-3) |> ignore) |> should (throwWithMessage "Accuracy should not be negative") typeof<Exception>

[<Test>]
let ``Should work correctly``() =
    RoundingCounter 3 {
        let! a = 2.0 / 12.0
        let! b = 3.5
        return a / b
    } |> should (equalWithin 0.0001) 0.048

[<Test>]
let ``Should work with irrational numbers``() =
    RoundingCounter 2 {
        let! a = Math.PI
        let! b = Math.E
        return a * b
    } |> should (equalWithin 0.0001) 8.54

[<Test>]
let ``Should work with negative numbers``() =
    RoundingCounter 3 {
        let! a = -1.11
        let! b = 56.234
        return a * b
    } |> should (equalWithin 0.0001) -62.420

[<Test>]
let ``Should work with complicated computation``() =
    RoundingCounter 2 {
        let! a = 0.345
        let! b = -2.34 / a
        let! c = Math.PI - a * b
        let! c = c * b
        return a + b + c
    } |> should (equalWithin 0.0001) -44.24
