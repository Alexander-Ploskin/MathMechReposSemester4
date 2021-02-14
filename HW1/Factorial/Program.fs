/// Calculates factorial of a number
let rec factorial number =
    if (number < 0) then raise(System.ArgumentException("number should not be negative"))
    elif (number < 2) then 1
    else number * factorial (number - 1)

printfn "%A" (factorial 5) 