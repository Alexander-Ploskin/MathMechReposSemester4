module LocalNetwork

open System
open Computer
open OS
open Virus

/// Network of computers with a virus which tries to infect all computers until it can infect anything
type LocalNetwork(computers : List<Computer>, virus : Virus, random : Random) =
    /// Make turn of the simulation
    member this.turn =
        computers |> List.iter (fun (computer : Computer) -> computer.tryInfect(virus, random))
        List.iter (fun (computer : Computer) -> computer.JustInfected <- false) computers

    /// Checks if network can be changed
    member this.canChange =
        List.exists (fun (comp : Computer) -> (comp.canBeInfected(virus))) computers

    /// Runs simulation
    member this.run =
        if this.canChange then
            this.turn
            let infectedString (comp : Computer) = if comp.Infected then "infected" else "not infected"
            List.iter (fun (comp : Computer) -> printfn $"{comp.Name} with OS {comp.OS} is {infectedString comp}") computers
            printfn "Press any button to continue\n"
            Console.ReadKey() |> ignore
            this.run
        else 
            printfn "Simulation is finished"
