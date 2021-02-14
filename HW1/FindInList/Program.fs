// Finds a first position of the element in the list
let findPosition list value =
    let rec loop list countdown =
       match list with
       | head :: tail when head = value -> Some(list.Length - countdown)
       | head :: tail -> loop tail (countdown - 1)
       | [] -> None
    loop list 0

printfn "%A" (findPosition [ 1..10000 ] 9999)