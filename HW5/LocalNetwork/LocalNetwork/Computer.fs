module Computer

open OS

/// Computer in the network
type Computer(os : OS) =
    member val OS = os with get
    member val Infected = false with get, set
