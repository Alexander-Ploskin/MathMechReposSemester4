module LocalNetworkTests

open System
open FsCheck
open FsUnit
open NUnit.Framework
open LocalNetwork
open Computer
open OS
open Virus

type MyRandom (x) = 
   inherit Random()
   override rand.NextDouble() = x


let fwf = LocalNetwork([], Virus())
[<Test>]
let Test1 () =
    Assert.Pass()
    