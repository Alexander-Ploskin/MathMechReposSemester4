module SquarePrinter

// Creates string of symbols bordered by the specified symbols
let getBorderedString len border content =
    border + (List.fold (fun acc c -> acc + c) "" [ for j in 1..len-2 -> content]) + border + "\n"

// Creates string of square with the specified length
let stringSquare n =
    let rec loop i acc =
        match i with
        | 0 -> acc
        | _ -> loop (i - 1) (acc + getBorderedString n "*" "1")
    if n = 1 then "*"
    else (getBorderedString n "*" "*") + (loop (n - 2) "") + (getBorderedString n "*" "*")

printfn "%A" <| stringSquare 5
