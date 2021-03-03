module SequenceOfPrimes

/// <summary>
/// Checks if number is prime
/// </summary
/// <param name="number">Number to check</param>
let isPrime (number : bigint) =
    seq { bigint 2 .. bigint(System.Math.Sqrt(float number))}
    |> Seq.exists (fun x -> number % x = bigint 0)
    |> not

/// <summary>
/// // Creates infinity sequence of prime numbers
/// </summary
let primes () =
    Seq.initInfinite (fun i -> bigint i + bigint 2)
    |> Seq.filter isPrime
