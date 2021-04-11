module LazyTests

open FsUnit
open NUnit.Framework
open LazyFactory
open ILazy
open System.Threading

[<Test>]
let ``Single threaded lazy should work correctly`` () =
    let lazyInstance = LazyFactory.CreateSingleThreadedLazy(fun () -> 2 + 2)
    lazyInstance.Get() |> should equal 4
    lazyInstance.Get() |> should equal 4

[<Test>]
let ``Simple lazy should calculate once`` () =
    let counter = ref 0L
    let lazyInstance = LazyFactory.CreateSingleThreadedLazy(fun () -> Interlocked.Increment counter)
    lazyInstance.Get() |> should equal 1
    lazyInstance.Get() |> should equal 1
    counter.Value |> should equal 1

[<Test>]
let ``Multi threaded lazy should work correctly`` () =
    let lazyInstance = LazyFactory.CreateMultiThreadedLazy(fun () -> 2 + 2)
    lazyInstance.Get() |> should equal 4
    lazyInstance.Get() |> should equal 4

[<Test>]
let ``Multi threaded lazy should calculate once in a single thread`` () =
    let counter = ref 0L
    let lazyInstance = LazyFactory.CreateMultiThreadedLazy(fun() -> Interlocked.Increment counter)
    lazyInstance.Get() |> should equal 1
    lazyInstance.Get() |> should equal 1
    counter.Value |> should equal 1

[<Test>]
let ``Multi threaded lazy should calculate once if there are a lot of threads`` () =
    let counter = ref 0L
    let lazyInstance = LazyFactory.CreateMultiThreadedLazy(fun () -> Interlocked.Increment counter)
    lazyInstance.Get() |> should equal 1
    ThreadPool.QueueUserWorkItem(fun obj -> lazyInstance.Get() |> should equal 1) |> ignore
    counter.Value |> should equal 1

[<Test>]
let ``Lock-free lazy should work correctly`` () =
    let lazyInstance = LazyFactory.CreateLockFreeLazy(fun () -> 2 + 2)
    lazyInstance.Get() |> should equal 4
    lazyInstance.Get() |> should equal 4
