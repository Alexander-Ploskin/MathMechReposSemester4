module MultiThreadedLazy

open System
open ILazy

/// Thread safe implementation of lazy calculation
type MultiThreadedLazy<'a> (supplier : unit -> 'a) =
    [<VolatileField>]
    let mutable calculated = false
    let mutable result = None
    let lockObject = new Object()

    let calculate =
        if calculated |> not then
            calculated <- true
            result <- Some(supplier())

    interface ILazy<'a> with
        member this.Get() =
            if calculated |> not then lock lockObject (fun () -> calculate)
            result.Value
