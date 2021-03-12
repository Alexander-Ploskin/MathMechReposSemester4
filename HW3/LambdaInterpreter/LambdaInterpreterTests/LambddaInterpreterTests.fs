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

        let I x = Abstraction(x, Variable x)
        let K x y = Abstraction(x, Abstraction(y, Variable x))
        let K_Star x y = Abstraction(x, Abstraction(y, Variable y))
        [|
            Application(I x, I x),
            I x
            
            Application(K x y, I x),
            K_Star y x
            
            Application(K x y, Application(K_Star x y, K x y)),
            K y x

            Application(K_Star x y, Application(I x, I x)),
            I y
        |]

    [<TestCaseSource(nameof LambdaInterpreterTests.testCases)>]
    member this.``Should work correct on simple examples`` (testCase: LambdaTerm<Guid> * LambdaTerm<Guid>) =
        let term, expectedNormalForm = testCase
        reduce term |> should equal expectedNormalForm
