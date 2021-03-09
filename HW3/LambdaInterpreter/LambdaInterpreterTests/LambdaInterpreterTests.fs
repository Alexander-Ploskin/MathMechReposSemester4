module LambdaInterpreterTests

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
    static member SimpleExamples = [|
        Application(S, Application(K, K)), I
        Application(I, I), I
        Application(K, I), K_Star
    |]

    member this.areSame term1 term2 = 
        match term1, term2 with
        | Variable value1, Variable value2 -> true
        | Application(left1, right1), Application(left2, right2) -> this.areSame left1 left2 && this.areSame right1 right2
        | Abstraction(variable1, innerTerm1), Abstraction(variable2, innerTerm2) -> variable1 = variable2 

    [<TestCaseSource("SimpleExamples")>]
    [<Test>]
    member this.``Should be correct on simple examples`` testCase =
        let term, expected = testCase
        reduce term |> should equal expected
