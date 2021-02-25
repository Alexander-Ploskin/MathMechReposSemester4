module ExpressionCalculatorTests

open ExpressionCalculator
open NUnit.Framework
open FsUnit

type ExpressionCalculatorTests () =
    static member SimpleExpressions = [|
        Number 1, 1
        Sum(Number 1, Number 2), 3
        Difference(Number 1, Number 2), -1
        Quotient(Number 4, Number 2), 2
        Product(Number 2, Number -3), -6
    |]
        
    [<TestCaseSource("SimpleExpressions")>]
    [<Test>]
    member this.``Should calculate simple expressions`` (testCase) =
        let expression, expected = testCase
        calculate expression |> should equal expected
    
    [<Test>]
    member this. ``Should calculate expression with children`` () =
        let expression = Sum(Product(Number 2, Number 3), Quotient(Number 4, Number 2))
        calculate expression |> should equal 8
    
    [<Test>]
    member this.``Should throw exception if expression contains division by zero`` () =
        let expression = Quotient(Number 1, Number 0)
        (fun () -> calculate expression |> ignore) |> should throw typeof<System.DivideByZeroException>

    [<Test>]
    member this.``Should throw exception if expression contains division by zero in child`` () =
        let expression = Quotient(Number 1, Sum(Number 1, Number -1))
        (fun () -> calculate expression |> ignore) |> should throw typeof<System.DivideByZeroException>
