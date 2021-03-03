module SequenceOfPrimes

// Checks if number is prime
let isPrime (number : bigint) =
    seq { bigint 2 .. bigint(System.Math.Sqrt(float number))}
    |> Seq.exists (fun x -> number % x = bigint 0)
    |> not

// Creates infinity sequence of prime numbers
let primes () =
    Seq.initInfinite (fun i -> bigint i + bigint 2)
    |> Seq.filter isPrime
