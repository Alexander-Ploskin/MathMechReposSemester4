module LazyFactory

open SingleThreadedLazy
open MultiThreadedLazy
open LockFreeLazy
open ILazy

// Provides creation of three types of lazy calculations
type LazyFactory =
    // Creates the single threaded implementation of lazy calculation
    static member CreateSingleThreadedLazy supplier =
        new SingleThreadedLazy<'a>(supplier) :> ILazy<'a>

    // Creates the thread safe implementation of lazy calculation
    static member CreateMultiThreadedLazy supplier =
        new MultiThreadedLazy<'a>(supplier) :> ILazy<'a>

    // Creates the lock-free thread safe implementation of lazy calculation
    static member CreateLockFreeLazy supplier =
        new LockFreeLazy<'a>(supplier) :> ILazy<'a>
