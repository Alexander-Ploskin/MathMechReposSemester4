module PointFreeTests

open NUnit.Framework
open FsCheck

open PointFree

[<Test>]
let ``Should have same output with initial function`` () =
    Check.Quick (fun number list -> (multiplyByNumber number list) = (multiplyByNumber'3 number list))
