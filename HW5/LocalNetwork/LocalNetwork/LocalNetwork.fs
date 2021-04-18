module LocalNetwork

open System
open Computer
open OS
open Virus

type LocalNetwork(computers : List<Computer>, virus : Virus, random : Random) =
    member this.turn  =
        let tryInfect (computer : Computer) =
            match computer.Infected with
            | true -> ()
            | false -> computer.tryInfect(virus, random)

        List.iter tryInfect computers
        List.iter (fun (computer : Computer) -> computer.JustInfected <- false) computers

    member this.canChange =
        List.exists (fun (comp : Computer) -> (comp.canBeInfected(virus))) computers

    member this.run =
        if this.canChange  then
            this.turn
            let infectedString (comp : Computer) = if comp.Infected then "infected" else "not infected"
            List.iter (fun (comp : Computer) -> printfn $"{comp.Name} with OS {comp.OS} is {infectedString comp}") computers
            printfn "Press any button to continue\n"
            Console.ReadKey() |> ignore
            this.run
        else 
            printfn "Simulation is finished"
