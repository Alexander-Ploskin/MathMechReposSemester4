module EvenNumberCounters

open NUnit.Framework
open FsCheck
open FsUnit
open Counters

[<Test>]
let ``Should work on empty list``() =
    let list = []
    mapEvenNumbersCounter list |> should equal 0
    filterEvenNumbersCounter list |> should equal 0
    foldEvenNumbersCounter list |> should equal 0

[<Test>]
let ``Should work on list of natural numbers``() =
    let list = [1; 2; 3; 4; 5]
    mapEvenNumbersCounter list |> should equal 2
    filterEvenNumbersCounter list |> should equal 2
    foldEvenNumbersCounter list |> should equal 2

[<Test>]
let ``Should work with zero``() =
    let list = [1; 0; 1]
    mapEvenNumbersCounter list |> should equal 1
    filterEvenNumbersCounter list |> should equal 1
    foldEvenNumbersCounter list |> should equal 1

[<Test>]
let ``Should work on negative numbers``() =
    let list = [-1; -2; -3; -4; -5]
    mapEvenNumbersCounter list |> should equal 2
    filterEvenNumbersCounter list |> should equal 2
    foldEvenNumbersCounter list |> should equal 2

[<Test>]
let ``Should have same output``() = 
    let checkIfHaveSameOutput list =
        let mapResult = mapEvenNumbersCounter list
        let filterResult = filterEvenNumbersCounter list
        let foldResult = foldEvenNumbersCounter list
        mapResult = filterResult && filterResult = foldResult
    Check.QuickThrowOnFailure checkIfHaveSameOutput
