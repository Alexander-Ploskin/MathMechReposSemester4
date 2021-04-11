module LazyFactory

open SingleThreadedLazy
open MultiThreadedLazy
open LockFreeLazy
open ILazy

type LazyFactory =
    static member CreateSingleThreadedLazy supplier =
        new SingleThreadedLazy<'a>(supplier) :> ILazy<'a>

    static member CreateMultiThreadedLazy supplier =
        new MultiThreadedLazy<'a>(supplier) :> ILazy<'a>

    static member CreateLockFreeLazy supplier =
        new LockFreeLazy<'a>(supplier) :> ILazy<'a>
