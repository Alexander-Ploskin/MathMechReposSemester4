namespace EvenNumbersCounters

//Contains functions that counts amount of even numbers in list
module Counters =
    let mapEvenNumbersCounter list =
        List.map (fun number -> (abs(number) + 1) % 2) list |> List.sum

    let filterEvenNumbersCounter list = 
        List.filter (fun number -> number % 2 = 0) list |> List.length

    let foldEvenNumbersCounter list = 
        List.fold (fun acc number -> acc + (abs(number) + 1) % 2) 0 list
