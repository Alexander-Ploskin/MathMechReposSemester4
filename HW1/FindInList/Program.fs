// Finds a first position of the element in the list
let findPosition list value =
    let rec loop list i =
       match list with
       | head :: tail when head = value -> Some(i)
       | head :: tail -> loop tail (i + 1)
       | [] -> None
    loop list 0

printfn "%A" (findPosition [ 1..10000 ] 7001)