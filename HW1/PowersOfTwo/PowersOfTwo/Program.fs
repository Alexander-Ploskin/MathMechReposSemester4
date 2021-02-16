// Creates list of all powers of two from n to n + m
let getPowersOfTwo n m =
    let result = [1]
    if (m < 0 or n < 0) then raise(System.ArgumentException("arguments should not be negative"))
    let rec loop acc countdown =
        if (countdown = 0) then acc
        elif (countdown <= n) then loop (List.map (fun x -> x * 2) acc) (countdown - 1)
        else loop ((acc.Head * 2) :: acc) (countdown - 1)
    List.rev (loop result (m + n))
    
printfn "%A" (getPowersOfTwo 7 4)