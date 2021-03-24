module OS

type OS =
    | Windows
    | Linux
    | MacOS

type InfectionInfo() =
    static member Check = function
        | (Windows, Windows) -> 0.9
        | (Linux, Linux) -> 0.75
        | (MacOS, MacOS) -> 0.34
        | (Windows, Linux) -> 0.5
        | (Linux, Windows) -> 0.4
        | (Windows, MacOS) -> 0.2
        | (MacOS, Windows) -> 0.15
        | (MacOS, Linux) -> 0.8
        | (Linux, MacOS) -> 0.67
