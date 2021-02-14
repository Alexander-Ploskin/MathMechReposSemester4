// Calculates the Fibonacci number by an index
let fibonacci number =
    if (number < 0) then raise(System.ArgumentException("number should not be negative"))
    let rec loop first second countdown =
        if (countdown = 0) then first
        else loop second (first + second) (countdown - 1)
    loop 0 1 number
    
printfn "%A" (fibonacci 10)
