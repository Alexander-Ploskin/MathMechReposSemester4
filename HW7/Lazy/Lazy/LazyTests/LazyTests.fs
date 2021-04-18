module LazyTests

open FsUnit
open NUnit.Framework
open LazyFactory
open ILazy
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic

[<Test>]
let ``Single threaded lazy should work correctly`` () =
    let lazyInstance = LazyFactory.CreateSingleThreadedLazy(fun () -> 2 + 2)
    lazyInstance.Get() |> should equal 4
    lazyInstance.Get() |> should equal 4

[<Test>]
let ``Simple lazy should calculate once`` () =
    let counter = ref 0
    let lazyInstance = LazyFactory.CreateSingleThreadedLazy(fun () -> counter.Value <- counter.Value + 1)
    lazyInstance.Get() |> ignore
    lazyInstance.Get() |> ignore
    counter.Value |> should equal 1

[<Test>]
let ``Multi threaded lazy should work correctly`` () =
    let lazyInstance = LazyFactory.CreateMultiThreadedLazy(fun () -> 2 + 2)
    lazyInstance.Get() |> should equal 4
    lazyInstance.Get() |> should equal 4

[<Test>]
let ``Multi threaded lazy should calculate once in a single thread`` () =
    let counter = ref 0
    let lazyInstance = LazyFactory.CreateMultiThreadedLazy(fun() -> counter.Value <- counter.Value + 1)
    lazyInstance.Get() |> ignore
    lazyInstance.Get() |> ignore
    counter.Value |> should equal 1

[<Test>]
let ``Multi threaded lazy should calculate once if there are a lot of threads`` () =
    let counter = ref 0L
    let lazyInstance = LazyFactory.CreateMultiThreadedLazy(fun () -> counter.Value <- Interlocked.Increment counter)
    lazyInstance.Get() |> ignore
    Parallel.For(0, 10, (fun obj -> lazyInstance.Get() |> ignore)) |> ignore
    counter.Value |> should equal 1

[<Test>]
let ``Lock-free lazy should work correctly`` () =
    let lazyInstance = LazyFactory.CreateLockFreeLazy(fun () -> 2 + 2)
    lazyInstance.Get() |> should equal 4
    lazyInstance.Get() |> should equal 4

[<Test>]
let ``Lock-free lazy in a single thread should return the same object`` () =
    let lazyInstance = LazyFactory.CreateLockFreeLazy(fun () -> new List<int>())
    let list1 = lazyInstance.Get()
    let list2 = lazyInstance.Get()
    list1.Add(1)
    list1 |> should equal list2

[<Test>]
let ``Lock-free lazy in a lot of threads should return the same object`` () =
    let lazyInstance = LazyFactory.CreateLockFreeLazy(fun () -> new List<int>())
    let list = lazyInstance.Get()
    list.Add(1)
    Parallel.For(0, 10, (fun obj -> lazyInstance.Get() |> should equal list)) |> ignore
