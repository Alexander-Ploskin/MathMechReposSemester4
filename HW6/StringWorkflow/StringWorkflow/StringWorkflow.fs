module StringWorkflow

open System

// Workflow builder wich provides integer calculation of strings
type StringCalcBuilder() =
    member this.Bind (x : string, f) =
        match Int32.TryParse x with
        | (true, value) -> f value
        | _ -> None
    member this.Return x =
        Some(x)
