module BracketBalanceChecker

/// Defines current state of loop
type BalanceState =
    { Square: int;
      Round: int;
      Curly: int; }

/// Checks if string is balanced
let isBalanced string =
  let rec loop state = function
    | ']'::_  when state.Square = 0 -> false
    | ']'::tail -> loop ({Square = state.Square - 1; Round = state.Round; Curly = state.Curly}) tail
    | '['::tail -> loop ({Square = state.Square + 1; Round = state.Round; Curly = state.Curly}) tail
    | ')'::_  when state.Round = 0 -> false
    | ')'::tail -> loop ({Square = state.Square; Round = state.Round - 1; Curly = state.Curly}) tail
    | '('::tail -> loop ({Square = state.Square; Round = state.Round + 1; Curly = state.Curly}) tail
    | '}'::_  when state.Curly = 0 -> false
    | '}'::tail -> loop ({Square = state.Square; Round = state.Round; Curly = state.Curly - 1}) tail
    | '{'::tail -> loop ({Square = state.Square; Round = state.Round; Curly = state.Curly + 1}) tail
    | [] -> state.Round = 0 && state.Square = 0 && state.Curly = 0
    | _::tail -> loop state tail
 
  string |> Seq.toList |> loop {Square = 0; Round = 0; Curly = 0}
