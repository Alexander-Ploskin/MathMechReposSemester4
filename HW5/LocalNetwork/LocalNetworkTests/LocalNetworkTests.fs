module LocalNetworkTests

open System
open FsUnit
open Foq
open NUnit.Framework
open LocalNetwork
open Computer
open OS
open Virus

[<TestFixture>]
type LocalNetworkTests() = 
    let oneRandom = 
       Mock<System.Random>()
        .Setup(fun x -> <@ x.NextDouble() @>).Returns(0.99)
        .Create()

    let zeroRandom = 
        Mock<System.Random>()
         .Setup(fun x -> <@ x.NextDouble() @>).Returns(0.0)
         .Create()

    [<DefaultValue>] val mutable simpleNet : List<Computer>
    [<DefaultValue>] val mutable infectiousVirus : Virus
    [<DefaultValue>] val mutable notInfectiousVirus : Virus

    [<SetUp>]
    member this.SetUp () =
        let infectiousVirus = Virus()
        infectiousVirus.WindowsToWindows <- 1.0
        this.infectiousVirus <- infectiousVirus

        let notInfectiousVirus = Virus()
        notInfectiousVirus.WindowsToWindows <- 0.0
        this.notInfectiousVirus <- notInfectiousVirus

        let computer11 = Computer(Windows, "comp1")
        let computer12 = Computer(Windows, "comp2")
        let computer13 = Computer(Windows, "comp3")
        computer11.AdjacentComputers <- [computer12]
        computer12.AdjacentComputers <- [computer11; computer13]
        computer13.AdjacentComputers <- [computer12]

        this.simpleNet <- [computer11; computer12; computer13]

    [<Test>]
    member this. ``Can continue should be false if probability of infection is 0`` () =
        (this.simpleNet.Item 0).Infected <- true
        LocalNetwork(this.simpleNet, this.notInfectiousVirus, new Random()).canChange |> should be False

    [<Test>]
    member this. ``Can continue should be true if there is probability of infection`` () =
        (this.simpleNet.Item 0).Infected <- true
        LocalNetwork(this.simpleNet, this.infectiousVirus, new Random()).canChange |> should be True

    [<Test>]
    member this.``Can execute should be false if there is not ane infected computer`` () =
        LocalNetwork(this.simpleNet, this.notInfectiousVirus, new Random()).canChange |> should be False

    [<Test>]
    member this.``Can change should be false if infected computer is not linked to any other not infected`` () =
        (this.simpleNet.Item 0).Infected <- true
        (this.simpleNet.Item 0).AdjacentComputers <- []
        LocalNetwork(this.simpleNet, this.notInfectiousVirus, new Random()).canChange |> should be False

    [<Test>]
    member this.``Can change should be false if there is not any not infected computer`` () =
        List.iter (fun (comp : Computer) -> comp.Infected <- true) this.simpleNet
        LocalNetwork(this.simpleNet, this.notInfectiousVirus, new Random()).canChange |> should be False

    [<Test>]
    member this.``Turn should do nothing if there is not any infected computer`` () =
        LocalNetwork(this.simpleNet, this.infectiousVirus, new Random()).turn
        List.fold (fun acc (comp : Computer) -> acc || comp.Infected) false this.simpleNet |> should be False

    [<Test>]
    member this.``Turn should do nothing if probability of infection is 0`` () =
        (this.simpleNet.Item 0).Infected <- true
        LocalNetwork(this.simpleNet, this.notInfectiousVirus, new Random()).turn
        List.fold (fun acc (comp : Computer) -> acc || (comp.Infected && (comp.Name = "comp1" |> not))) false this.simpleNet |> should be False

    [<Test>]
    member this.``Should do nothing if infected computer isn't linked to any uninfected computer`` () =
        (this.simpleNet.Item 0).Infected <- true
        (this.simpleNet.Item 0).AdjacentComputers <- []
        LocalNetwork(this.simpleNet, this.notInfectiousVirus, new Random()).turn
        List.fold (fun acc (comp : Computer) -> acc || (comp.Infected && (comp.Name = "comp1" |> not))) false this.simpleNet |> should be False

    [<Test>]
    member this.``Should do BFS if probability is 1`` () =
        let computer21 = Computer(Windows, "comp1")
        let computer22 = Computer(Windows, "comp2")
        let computer23 = Computer(Windows, "comp3")
        let computer24= Computer(Windows, "comp4")
        let computer25 = Computer(Windows, "comp5")

        computer21.AdjacentComputers <- [computer22; computer23]
        computer22.AdjacentComputers <- [computer21; computer24; computer25]
        computer23.AdjacentComputers <- [computer21]
        computer24.AdjacentComputers <- [computer22]
        computer25.AdjacentComputers <- [computer22]
        computer21.Infected <- true
        let network = LocalNetwork([computer21; computer22; computer23; computer24; computer25], this.infectiousVirus, new Random())

        network.turn
        (computer21.Infected && computer22.Infected && computer23.Infected) |> should be True

        network.turn
        (computer21.Infected && computer22.Infected && computer23.Infected && computer24.Infected && computer25.Infected) |> should be True

    [<Test>]
    member this.``Should do parralel BFS if net is not linked`` () =
        let computer31 = Computer(Windows, "comp1")
        let computer32 = Computer(Windows, "comp2")
        let computer33 = Computer(Windows, "comp3")
        let computer34= Computer(Windows, "comp4")

        computer31.AdjacentComputers <- [computer32]
        computer32.AdjacentComputers <- [computer31]
        computer33.AdjacentComputers <- [computer34]
        computer34.AdjacentComputers <- [computer33]

        computer31.Infected <- true
        computer33.Infected <- true
        
        let network = LocalNetwork([computer31; computer32; computer33], this.infectiousVirus, new Random())
        network.turn
        (computer31.Infected && computer32.Infected && computer33.Infected) |> should be True

    [<Test>]
    member this.``Should do BFS with default virus and random of 1.0`` () =
        let computer21 = Computer(Windows, "comp1")
        let computer22 = Computer(Linux, "comp2")
        let computer23 = Computer(MacOS, "comp3")
        let computer24= Computer(Windows, "comp4")
        let computer25 = Computer(Linux, "comp5")

        computer21.AdjacentComputers <- [computer22; computer23]
        computer22.AdjacentComputers <- [computer21; computer24; computer25]
        computer23.AdjacentComputers <- [computer21]
        computer24.AdjacentComputers <- [computer22]
        computer25.AdjacentComputers <- [computer22]
        computer21.Infected <- true
        let network = LocalNetwork([computer21; computer22; computer23; computer24; computer25], Virus(), zeroRandom)

        network.turn
        (computer21.Infected && computer22.Infected && computer23.Infected) |> should be True

        network.turn
        (computer21.Infected && computer22.Infected && computer23.Infected && computer24.Infected && computer25.Infected) |> should be True

    [<Test>]
    member this.``Should do nothing with default virus and random of 0.0`` () =
        (this.simpleNet.Item 0).Infected <- true
        LocalNetwork(this.simpleNet, Virus(), oneRandom).turn
        List.fold (fun acc (comp : Computer) -> acc || (comp.Infected && (comp.Name = "comp1" |> not))) false this.simpleNet |> should be False
