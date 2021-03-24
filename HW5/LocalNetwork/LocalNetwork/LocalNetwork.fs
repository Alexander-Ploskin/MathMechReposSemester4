module LocalNetwork

open System
open Computer
open OS

type LocalNetwork(computers : List<Computer>) =
    let turn(rand : Random) =
        let tryInfect (computer : Computer) =
            match computer.Infected with
            | true -> ()
            | false -> computer.Infected <- List.exists (fun (adjacentComp : Computer) -> (adjacentComp.Infected && (adjacentComp.JustInfected |> not) && (rand.NextDouble() < InfectionInfo.Check(computer.OS, adjacentComp.OS)))) computer.AdjacentComputers
                       if computer.Infected then computer.JustInfected <- true

        List.iter tryInfect computers
        List.iter (fun (computer : Computer) -> computer.JustInfected <- false) computers

    let canChange =
        let canChangeNode (comp : Computer) =
            comp.Infected && List.exists (fun (adjacentComp : Computer) -> (adjacentComp.Infected |> not) && InfectionInfo.Check(comp.OS, adjacentComp.OS) > 0.0) comp.AdjacentComputers
        List.exists (fun (comp : Computer) -> (canChangeNode comp))

    member this.run =
        if canChange computers then
            turn <| new Random()
            let infectedString (comp : Computer) = if comp.Infected then "infected" else "not infected"
            List.iter (fun (comp : Computer) -> printfn $"{comp.Name} with OS {comp.OS} is {infectedString comp}") computers
            printfn "Press any button to continue\n"
            Console.ReadKey() |> ignore
            this.run
        else 
            printfn "Simulation is finished"
