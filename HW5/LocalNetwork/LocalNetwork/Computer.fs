module Computer

open OS

type Computer(os : OS, name : string) =
    member val OS = os with get
    member val Infected = false with get, set
    member val AdjacentComputers : List<Computer> = [] with get, set
    member val JustInfected = false with get, set
    member val Name = name with get
