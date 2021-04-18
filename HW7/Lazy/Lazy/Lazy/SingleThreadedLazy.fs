module SingleThreadedLazy

open ILazy

/// The simple not thread safe implementation of lazy calculation
type SingleThreadedLazy<'a> (supplier : unit -> 'a) =
    let mutable result = None

    interface ILazy<'a> with
        member this.Get() =
            if result.IsNone then
                result <- Some(supplier())
            result.Value
