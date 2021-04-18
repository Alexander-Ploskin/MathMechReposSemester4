module Computer

open OS
open System
open Virus

/// Provides information about computer in a network
type Computer(os : OS, name : string) =
    member val OS = os with get
    member val Infected = false with get, set
    member val AdjacentComputers : List<Computer> = [] with get, set
    member val JustInfected = false with get, set
    member val Name = name with get

    /// Tries to transmit infection to this 
    member this.tryInfect(virus : Virus, random : Random) =
        if this.Infected |> not then
            this.Infected <- this.AdjacentComputers |> List.exists (fun (adjacentComp : Computer) -> 
            (adjacentComp.Infected && (adjacentComp.JustInfected |> not) && (random.NextDouble() < virus.Check(this.OS, adjacentComp.OS))))
            if this.Infected then this.JustInfected <- true

    member this.canBeInfected(virus : Virus) =
        this.Infected && List.exists (fun (adjacentComp : Computer) -> (adjacentComp.Infected |> not) && virus.Check(this.OS, adjacentComp.OS) > 0.0) this.AdjacentComputers
