module HelpersTests

open System
open NUnit.Framework
open FsUnit
open FsCheck

open LambdaInterpreter

let x = Guid.NewGuid()
let y = Guid.NewGuid()
let z = Guid.NewGuid()

[<Test>]
let ``New variables generator should work correct``() =
    let CheckIfNotContainedInSet set =
        set |> Set.contains (getNewValueNotFromTheSet set) |> not
    Check.QuickThrowOnFailure CheckIfNotContainedInSet

[<Test>]
let ``Just variables should be recognized as free``() =
    Application(Variable x, Abstraction(x, Application(Variable y, Variable z))) |> getFreeVariables |> should equal (set [x; y; z])

[<Test>]
let ``Variables in abstraction should not be recognized as free``() =
    Application(Abstraction(x, Variable x), Abstraction(y, Variable y)) |> getFreeVariables |> should equal (Set.empty)

[<Test>]
let ``Substitution into same named variable should just change it``() =
    substitute (Variable x) x (Application(Variable y, Variable z)) |> should equal (Application(Variable y, Variable z))

[<Test>]
let ``Substitution into not same named variable should change nothing``() =
    substitute (Variable x) y (Application(Variable y, Variable z)) |> should equal (Variable x)

[<Test>]
let ``Substitution into application should be recursive``() =
    substitute (Application(Variable x, Variable x)) x (Variable z) |> should equal (Application(Variable z, Variable z))

[<Test>]
let ``Substitution into bounded variable should do nothing``() =
    substitute (Abstraction(x, Variable x)) x (Application(Variable y, Variable z)) |> should equal (Abstraction(x, Variable x))

[<Test>]
let ``Substitution whithout name collision should be successful``() =
    substitute (Abstraction(y, Variable x)) x (Variable z) |> should equal (Abstraction(y, Variable z))

[<Test>]
let ``Substitution with name collision should be successful`` () =
    let result = substitute (Abstraction(y, Variable x)) x (Variable y)
    match result with
    | Abstraction (_, term) -> 
        match term with
        | Variable name -> name |> should equal y
        | _ -> Assert.Fail()
    | _ -> Assert.Fail()
