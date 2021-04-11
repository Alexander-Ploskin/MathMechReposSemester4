module LockFreeLazy

open ILazy
open System.Threading

type SingleThreadedLazy<'a> (supplier : unit -> 'a) =
    let mutable calculated = false
    let mutable result = None

    interface ILazy<'a> with
        member this.Get() =
            if calculated |> not then
                let startValue = result
                let desiredValue = Some <| supplier()
                Interlocked.CompareExchange(&result, desiredValue, startValue) |> ignore
                calculated <- true
            result.Value
