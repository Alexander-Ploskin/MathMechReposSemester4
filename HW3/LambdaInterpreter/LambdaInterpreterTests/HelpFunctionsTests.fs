module HelpersTests

open System
open NUnit.Framework
open FsUnit
open FsCheck

open LambdaInterpreter

[<TestFixture>]
type HelpersTests() =
    [<Test>]
    member this.``New variables generator should work correct``() =
        let CheckIfNotContainedInSet set =
            set |> Set.contains (getNewValueNotFromTheSet set) |> not
        Check.QuickThrowOnFailure CheckIfNotContainedInSet

    static member GetFreeVariablesCases =
        let x = Guid.NewGuid()
        let y = Guid.NewGuid()
        let z = Guid.NewGuid()
        [|
            Application(Variable x, Abstraction(x, Application(Variable y, Variable z))), set [x; y; z]
            Application(Abstraction(x, Variable x), Abstraction(y, Variable y)), Set.empty
        |]

    static member SubstitutionCasesWithoutNameConflicts =
        let x = Guid.NewGuid()
        let y = Guid.NewGuid()
        let z = Guid.NewGuid()
        [|
            (Variable x), y, (Application(Variable y, Variable z)), Variable x
            (Application(Variable x, Variable y)), y, (Variable z), Application(Variable x, Variable z)
            (Abstraction(x, Variable x)), x, (Application(Variable y, Variable z)), Abstraction(x, Variable x)
            (Abstraction(y, Variable x)), x, (Variable z), Abstraction(y, Variable z)
        |]

    [<TestCaseSource(nameof(HelpersTests.GetFreeVariablesCases))>]
    member this.``Should get free variables correctly``(testCase: LambdaTerm<Guid> * Set<Guid>) =
        let term, expected = testCase
        term |> getFreeVariables |> should equal expected
    
    [<TestCaseSource(nameof(HelpersTests.SubstitutionCasesWithoutNameConflicts))>]
    member this.``Should substitute variables correctly``(testCase: LambdaTerm<Guid> * Guid * LambdaTerm<Guid> * LambdaTerm<Guid>) =
        let term, varToSubstitute, termToSubstitute, expected = testCase
        substitute term varToSubstitute termToSubstitute |> should equal expected

    [<Test>]
    member this.``Substitution with name collision should be successful`` () =
        let x = Guid.NewGuid()
        let y = Guid.NewGuid()
        let result = substitute (Abstraction(y, Variable x)) x (Variable y)
        match result with
        | Abstraction (_, term) -> 
            match term with
            | Variable name -> name |> should equal y
            | _ -> Assert.Fail()
        | _ -> Assert.Fail()
