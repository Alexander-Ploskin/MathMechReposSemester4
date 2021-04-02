module ThreadSafeStackTests

open FsUnit
open NUnit.Framework
open ThreadSafeStack

[<Test>]
let ``Should return none if stack is empty`` () =
    Stack<int>().TryPop() |> should equal None

[<Test>]
let ``Should correct push value in one thread`` () =
    let stack = Stack<string>()
    stack.Push("A")
    stack.TryPop() |> should equal <| Some("A")

