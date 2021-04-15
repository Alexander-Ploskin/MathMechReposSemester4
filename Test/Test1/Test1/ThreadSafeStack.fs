module ThreadSafeStack

//Thread-safe stack
type Stack<'a> () =
    let mutable storage = []

    //Sets new item on the head of stack
    member this.Push (item : 'a) =
        lock storage (fun () -> storage <- item :: storage)

    //Returns head and removes it from stack if stack is not empty else returns none
    member this.TryPop () =
        lock storage (fun () -> 
            match storage with
            | head :: newState -> 
                      storage <- newState
                      Some(head)
            | [] -> None
        )
