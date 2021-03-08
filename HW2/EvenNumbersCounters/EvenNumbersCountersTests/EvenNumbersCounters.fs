module EvenNumberCounters

open NUnit.Framework
open FsCheck
open FsUnit
open Counters

type EvenNumbersCountersTests () =
    let testingFunctions = [mapEvenNumbersCounter; filterEvenNumbersCounter; foldEvenNumbersCounter]
    static member testCases = [|
        [], 0
        [1; 2; 3; 4; 5], 2
        [1; 0; 1], 1
        [-1; -2; -3; -4; -5], 2
    |]

    [<TestCaseSource("testCases")>]
    [<Test>]
    member this.``Should work on simple lists`` testCase =
        let input, expected = testCase
        testingFunctions |> List.iter (fun x -> (x input) |> should equal expected)

    [<Test>]
    member this. ``Should have same output``() = 
        let checkIfHaveSameOutput list =
            let mapResult = mapEvenNumbersCounter list
            let filterResult = filterEvenNumbersCounter list
            let foldResult = foldEvenNumbersCounter list
            mapResult = filterResult && filterResult = foldResult
        Check.QuickThrowOnFailure checkIfHaveSameOutput
