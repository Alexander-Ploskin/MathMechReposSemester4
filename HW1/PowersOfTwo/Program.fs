// Creates list of all powers of five from n to m
let getPowersOfTwo n m =
    let result = [1]
    if (m < 0 or n < 0) then raise(System.ArgumentException("arguments should not be negative"))
    if (m < n) then raise(System.ArgumentException("lower border of the interval should be less than upper border"))
    let rec loop acc countdown =
        if (countdown = 0) then acc
        elif (countdown <= n) then loop (List.map (fun x -> x * 2) acc) (countdown - 1)
        else loop ((acc.Head * 2) :: acc) (countdown - 1)
    List.fold (fun acc elem -> elem::acc) [] (loop result m)
    
printfn "%A" (getPowersOfTwo 5 20)
        