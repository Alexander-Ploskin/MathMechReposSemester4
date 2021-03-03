/// <summary>
/// Contains functions that counts amount of even numbers in list
/// </summary>
module Counters

/// <summary>
/// Counts even numbers using map function
/// </summary>
let mapEvenNumbersCounter list =
    List.map (fun number -> (abs(number) + 1) % 2) list |> List.sum

/// <summary>
/// Counts even numbers using filterfunction
/// </summary>
let filterEvenNumbersCounter list = 
    List.filter (fun number -> number % 2 = 0) list |> List.length

/// <summary>
/// Counts even numbers using fold function
/// </summary>
let foldEvenNumbersCounter list = 
    List.fold (fun acc number -> acc + (abs(number) + 1) % 2) 0 list
