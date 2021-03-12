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
        [|
            Application (Abstraction (x, Variable y),
                Application (Abstraction (x, Application(Application(Variable x, Variable x), Variable x)),
                    Abstraction (x, Application(Application(Variable x, Variable x), Variable x)))),
            Variable y
            
            Application (Abstraction (x, Variable x), Abstraction (x, Variable x)),
            Abstraction(x, Variable x)
            
            Application (Abstraction (x, Abstraction (y, Variable x)), Abstraction (x, Variable x)),
            Abstraction (y, Abstraction (x, Variable x))
        |]

    [<TestCaseSource(nameof LambdaInterpreterTests.testCases)>]
    member this.``Terms with normal form must be reduced to it by normal reduction `` (testCase) =
        let term, expectedNormalForm = testCase
        reduce term |> should equal expectedNormalForm
