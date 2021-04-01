module StringWorkflowTests

open NUnit.Framework
open StringWorkflow
open FsUnit

let calculate = StringCalcBuilder()

[<Test>]
let ``Should return none if input contains not numbers strings`` () =
    calculate {
        let! a = "1"
        let! b = "Ъ"
        let c = a + b
        return c
    } |> should equal None

[<Test>]
let ``Should work correctly`` () =
    calculate {
        let! a = "1"
        let! b = "2"
        let c = a + b
        return c
    } |> should equal <| Some(3)

[<Test>]
let ``Should return none if input contains not integer numbers`` () =
    calculate {
        let! a = "1"
        let! b = "1.2"
        let c = a + b
        return c
    } |> should equal None

[<Test>]
let ``Should work correctly if result might be not integer`` () =
    calculate {
        let! a = "1"
        let! b = "2"
        let c = a / b
        return c
    } |> should equal <| Some(0)

[<Test>]
let ``Should work correctly on the complicated computation`` () =
    calculate {
    let! a = "3"
    let! b = "6"
    let c = b / a
    let d = 4 - a * b
    let d = c * b * d
    return a + b + c + d 
    } |> should equal <| Some(-157)
