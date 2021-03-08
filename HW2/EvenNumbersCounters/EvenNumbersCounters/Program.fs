/// Contains functions that counts amount of even numbers in 
module Counters

/// Counts even numbers using map function
let mapEvenNumbersCounter list =
    List.map (fun number -> (abs number + 1) % 2) list |> List.sum

/// Counts even numbers using filterfunction
let filterEvenNumbersCounter list = 
    List.filter (fun number -> number % 2 = 0) list |> List.length

/// Counts even numbers using fold 
let foldEvenNumbersCounter list = 
    List.fold (fun acc number -> acc + (abs number + 1) % 2) 0 list
