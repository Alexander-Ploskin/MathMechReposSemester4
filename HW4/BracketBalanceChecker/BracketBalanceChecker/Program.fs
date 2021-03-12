module BracketBalanceChecker

type BalanceState =
    { Square: int;
      Round: int;
      Curly: int; }

let faf = "fwefwafw3efr"
Seq.fold id "" 

let checkBracketBalance string =
    let rec loop (string : string) state =
        if state.Curly < 0 || state.Round < 0 || state.Square < 0 then false
        else match string with 
             | head :: tail when head = '[' -> loop tail (state.Square + 1)
             | head :: tail when head = ']' -> loop tail (state.Square - 1)
             | head :: tail when head = '(' -> loop tail (state.Square + 1)
             | head :: tail when head = ')' -> loop tail (state.Square - 1)
             | head :: tail when head = '{' -> loop tail (state.Square + 1)
             | head :: tail when head = '}' -> loop tail (state.Square - 1)
    loop string BalanceState {Square = 0; Round =  0; Curly = 0}
