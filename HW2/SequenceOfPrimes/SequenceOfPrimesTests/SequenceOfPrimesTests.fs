module SequenceOfPrimesTests

open SequenceOfPrimes
open NUnit.Framework
open Open.Numeric.Primes
open FsCheck

[<Test>]
let ``Should have correct number on any position`` () =
    let checkIsPrimeOnThePosition index=
        match index with
        | index when index >= 0 -> (primes() |> Seq.item index) = (Prime.Numbers |> Seq.item index |> bigint)
        | _ -> true
    Check.QuickThrowOnFailure checkIsPrimeOnThePosition
