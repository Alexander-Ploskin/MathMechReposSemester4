/// Calculates factorial of a number
let rec factorial number =
    if (number < 0) then invalidArg (nameof number) "number should not be negative"
    let rec loop acc countdown =
        if (countdown = 0) then acc
        else loop (acc * countdown) (countdown - 1)
    loop 1 number

printfn "%A" (factorial 6) 