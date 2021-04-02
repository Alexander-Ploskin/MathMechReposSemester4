module ThreadSafeStackTests

open FsUnit
open NUnit.Framework
open ThreadSafeStack

[<Test>]
let ``Should return none if stack is empty`` () =
    Stack<int>().TryPop() |> should equal None

[<Test>]
let ``Should correct push one value in one thread`` () =
    let stack = Stack<string>()
    stack.Push("A")
    stack.TryPop() |> should equal <| Some("A")

[<Test>]
let ``Should work correctly with many values`` () =
    let stack = Stack<int>()
    stack.Push(1)
    stack.Push(0)
    stack.TryPop() |> should equal <| Some("0")
    stack.TryPop() |> should equal <| Some("1")
