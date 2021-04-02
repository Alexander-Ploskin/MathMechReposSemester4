module SupermapTests

open FsUnit
open NUnit.Framework
open Supermap

[<Test>]
let ``Should work correctly on empty list`` () =
    supermap id list.Empty |> should equal list.Empty

[<Test>]
let ``Should work correctly as a simple map`` () =
    supermap (fun x -> [x + 2]) [1; 2] |> should equal [3; 4]

[<Test>]
let ``Should work correctly on the given example`` () =
    supermap (fun x -> [sin x; cos x]) [1.0; 2.0; 3.0] |> should equal [sin 1.0; cos 1.0; sin 2.0; cos 2.0; sin 3.0; cos 3.0]
