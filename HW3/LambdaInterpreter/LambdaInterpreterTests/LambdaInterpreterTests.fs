module LambdaInterpreterTests

open System
open NUnit.Framework
open FsUnit

open LambdaInterpreter

let I x = Abstraction(x, Variable x)
let K x y = Abstraction(x, Abstraction(y, Variable x))
let K_Star x y = Abstraction(x, Abstraction(y, Variable y))
let S x y z = Abstraction(x, Abstraction(y, Abstraction(z, Application(Variable x, Application(Variable z, Application(Variable y, Variable z))))))
let LowercaseOmega x = Abstraction(x, Application(Variable x, Variable x))
let Omega x = Application(LowercaseOmega x, LowercaseOmega x)

type LambdaInterpreterTests () =
    static member SimpleExamples = 
        let x = Guid.NewGuid()
        let y = Guid.NewGuid()
        let z = Guid.NewGuid()
        [|
            Application(S x y z, Application(K x y, K x y)), I x
            Application(I x, I x), I x
            Application(K x y, I x), K_Star x y
        |]

    [<TestCaseSource("SimpleExamples")>]
    [<Test>]
    member this.``Should be correct on simple examples`` testCase =
        let term, expected = testCase
        reduce term |> should equal expected
