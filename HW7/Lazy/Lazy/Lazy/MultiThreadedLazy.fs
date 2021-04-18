module MultiThreadedLazy

open System
open ILazy

/// Thread safe implementation of lazy calculation
type MultiThreadedLazy<'a> (supplier : unit -> 'a) =
    let mutable result = None
    let lockObject = new Object()

    let calculate =
        if result.IsNone then
            result <- Some(supplier())

    interface ILazy<'a> with
        member this.Get() =
            if result.IsNone |> not then lock lockObject (fun () -> calculate)
            result.Value
