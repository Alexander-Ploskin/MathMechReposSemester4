module BracketBalanceChecker

/// Checks if string is balanced by the definition given in list of pairs of opening and closing tokens
let isBalanced string pairs =
    let rec loop (state : List<char>) = function
        | token::tail when pairs |> List.exists (fun pair -> (fst pair) = token) -> loop (token :: state) tail
        | token::tail when pairs |> List.exists (fun pair -> (snd pair) = token) -> match state with
                                                                                    | head::newState when pairs
                                                                                        |> List.exists (fun pair -> (fst pair = head) && (snd pair = token)) -> loop newState tail
                                                                                    | _ -> false
        | _::tail -> loop state tail
        | [] -> state.Length = 0

    string |> Seq.toList |> loop []
