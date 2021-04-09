module LazyFactory

open SingleThreadedLazy
open ILazy

type LazyFactory =
    static member CreateSingleThreadedLazy supplier =
        new SingleThreadedLazy<'a>(supplier) :> ILazy<'a>
