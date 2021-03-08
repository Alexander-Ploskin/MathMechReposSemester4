module SequenceOfPrimes

/// Checks if number is prime
/// <param name="number">Number to check</param>
let isPrime (number : bigint) =
    seq { bigint 2 .. bigint(System.Math.Sqrt(float number))}
    |> Seq.exists (fun x -> number % x = 0I)
    |> not

/// <summary>
/// Creates infinity sequence of prime numbers
/// </summary
let primes () =
    Seq.initInfinite (fun i -> bigint i + bigint 2)
    |> Seq.filter isPrime
