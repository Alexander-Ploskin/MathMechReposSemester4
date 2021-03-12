module LambdaInterpreterTests

open System
open NUnit.Framework
open FsCheck
open FsUnit

open LambdaInterpreter

[<TestFixture>]
type LambdaInterpreterTests() =
    static member testCases() = 
        let x = Guid.NewGuid()
        let y = Guid.NewGuid()

        let I = Abstraction(x, Variable x)
        let K = Abstraction(x, Abstraction(y, Variable x))
        let K_Star = Abstraction(x, Abstraction(y, Variable y))
        [|
            Application(I, I),
            I
            
            Application(K, I),
            K_Star
            
            Application(K, Application(K_Star, K)),
            K

            Application(K_Star, Application(I, I)),
            I
        |]

    [<TestCaseSource(nameof LambdaInterpreterTests.testCases)>]
    member this.``Should work correct on simple examples`` (testCase: LambdaTerm<Guid> * LambdaTerm<Guid>) =
        let term, expectedNormalForm = testCase
        reduce term |> should equal expectedNormalForm
