module RoundingWorkflow

open System

// Workflow which provides rounding calculation with a specified accuracy
type RoundingCounter(accuracy : int) =
    do if accuracy < 0 then failwith "Accuracy should not be negative"
    member this.Bind (x : float, f) =
        f (Math.Round(x, accuracy))
    member this.Return(x : float) =
        Math.Round(x, accuracy)
