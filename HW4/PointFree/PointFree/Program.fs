module PointFree

/// Initial function
let multiplyByNumber x l = List.map (fun y -> y * x) l

/// Eta-converted
let multiplyByNumber'1 x = List.map (fun y -> y * x)

/// Wrote inner function using multiplication as a function
let multiplyByNumber'2 x = List.map ((*) x)

/// Eta-converted inner function
let multiplyByNumber'3 () = (*) >> List.map
