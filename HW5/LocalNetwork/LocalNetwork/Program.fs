module Program

open LocalNetwork
open Computer
open OS
open Virus

let computer1 = Computer(Windows, "c1")
let computer2 = Computer(Windows, "c2")
let computer3 = Computer(Windows, "c3")
let computer4 = Computer(Windows, "c4")
let computer5 = Computer(Windows, "c5")
let computer6 = Computer(Windows, "c6")
let computer7 = Computer(Windows, "c7")
let computer8 = Computer(Windows, "c8")
let computer9 = Computer(Windows, "c9")
let computer10 = Computer(Windows, "c10")
let computer11 = Computer(Windows, "c11")
let computer12 = Computer(Windows, "c12")
let computer13 = Computer(Windows, "c13")
let computer14 = Computer(Windows, "c14")
let computer15 = Computer(Windows, "c15")
let computers = [computer1; computer2; computer3; computer4; computer5; computer6; computer7;
computer8; computer9; computer10; computer11; computer12; computer13; computer14; computer15;]

computer1.AdjacentComputers <- [computer2; computer3; computer4; computer5]
computer2.AdjacentComputers <- [computer1; computer6; computer7]
computer3.AdjacentComputers <- [computer1; computer8; computer9]
computer4.AdjacentComputers <- [computer1; computer10; computer11]
computer5.AdjacentComputers <- [computer1; computer12; computer13]
computer6.AdjacentComputers <- [computer2]
computer7.AdjacentComputers <- [computer2]
computer8.AdjacentComputers <- [computer3]
computer9.AdjacentComputers <- [computer3]
computer10.AdjacentComputers <- [computer4]
computer11.AdjacentComputers <- [computer4]
computer12.AdjacentComputers <- [computer5]
computer13.AdjacentComputers <- [computer5]
computer14.AdjacentComputers <- [computer15]
computer15.AdjacentComputers <- [computer14]

computer1.Infected <- true
LocalNetwork(computers, Virus()).run
