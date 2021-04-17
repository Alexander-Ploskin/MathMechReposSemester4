module BracketBalanceCheckerTests

open NUnit.Framework
open FsUnit
open FsCheck

open BracketBalanceChecker

[<TestFixture>]
type BracketBalanceChecherTest() =
    static member testCases() = [|
        ("", true)
        (")", false)
        ("]", false)
        ("}", false)
        ("(", false)
        ("[", false)
        ("{", false)
        ("()", true)
        ("{}", true)
        ("[]", true)
        ("())", false)
        ("[]]", false)
        ("{}}", false)
        ("())(", false)
        ("[]][", false)
        ("{}}{", false)
        ("F)", false)
        ("(F)", true)
        ("()F", true)
        (" )", false)
        ("( )", true)
        ("() ", true)
        ("(]", false)
        ("[)", false)
        ("{]", false)
        (" ([)]", false)
    |]

    [<Test>]
    [<TestCaseSource("testCases")>]
    member this.``Should work correct on basic examples`` (testCase: string * bool) =
        let string, expected = testCase
        isBalanced string [('(', ')'); ('[', ']'); ('{', '}')] |> should equal expected
