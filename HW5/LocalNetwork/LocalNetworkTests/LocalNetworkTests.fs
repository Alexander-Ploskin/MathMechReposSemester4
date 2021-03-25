module LocalNetworkTests

open System
open FsCheck
open FsUnit
open NUnit.Framework
open LocalNetwork
open Computer
open OS


type MyRandom (x) = 
   inherit Random()
   override rand.NextDouble() = x

[<Test>]
let Test1 () =
    Assert.Pass()
    