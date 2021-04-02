module Supermap

// Links all lists given by maping list by the specified function
let supermap func list =
    List.map func list |> List.fold (fun acc list -> acc @ list) List.empty
