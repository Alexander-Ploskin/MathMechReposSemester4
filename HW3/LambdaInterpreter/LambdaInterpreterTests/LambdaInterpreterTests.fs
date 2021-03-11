module LambdaInterpreterTests

open System
open NUnit.Framework
open FsUnit

open LambdaInterpreter

let x = Guid.NewGuid()
let y = Guid.NewGuid()

let I = Abstraction(x, Variable x)
let K = Abstraction(x, Abstraction(y, Variable x))
let K_Star = Abstraction(x, Abstraction(y, Variable y))

let SimpleExamples = [|
    (Application(I, I))
    (Application(K, I))
    (Application(K, Application(K_Star, K)))
    (Application(K_Star, Application(I, I)))
|]

[<TestCaseSource(nameof(SimpleExamples))>]
let ``Should work correct on simple examples`` (testCase) =
    let term = testCase
    reduce term |> should equal I
