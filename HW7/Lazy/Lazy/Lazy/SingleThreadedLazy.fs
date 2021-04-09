module SingleThreadedLazy

open ILazy

type SingleThreadedLazy<'a> (supplier : unit -> 'a) =
    let mutable calculated = false
    let mutable result = None

    interface ILazy<'a> with
        member this.Get() =
            if calculated |> not then
                calculated <- true
                result <- Some(supplier())
            result.Value
