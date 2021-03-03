module MapForTreesTests

open NUnit.Framework
open FsCheck
open FsUnit
open MapForTrees

let rec areEqual tree1 tree2 =
    match tree1, tree2 with
    | Empty, Empty -> true
    | Tree(value1, left1, right1), Tree(value2, left2, right2) -> value1 = value2 && (areEqual left1 left2) && (areEqual right1 right2)
    | _ -> false

let rec haveSameStructure tree1 tree2 =
    match tree1, tree2 with
    | Empty, Empty -> true
    | Tree(_, left1, right1), Tree(_, left2, right2) -> (haveSameStructure left1 left2) && (haveSameStructure right1 right2)
    | _ -> false

[<Test>]
let ``Should work with empty tree``() =
    areEqual (treeMap Empty id) Empty |> should be True

[<Test>]
let ``Should work with leaf``() =
    areEqual (treeMap (Tree(2, Empty, Empty)) (fun x -> x * x)) (Tree(4, Empty, Empty)) |> should be True

[<Test>]
let ``Should work with children``() =
    let tree = Tree(2, (Tree(3, Empty, Empty)), (Tree(4, Empty, Empty)))
    let squaredTree = Tree(4, (Tree(9, Empty, Empty)), (Tree(16, Empty, Empty)))
    areEqual (treeMap tree (fun x -> x * x)) squaredTree |> should be True

[<Test>]
let ``Should work with grandchildren``() =
    let tree = Tree(2, (Tree(3, (Tree(4, Empty, Empty)), (Tree(5, Empty, Empty)))), (Tree(6, (Tree(7, Empty, Empty)), (Tree(8, Empty, Empty)))))
    let squaredTree = Tree(4, (Tree(9, (Tree(16, Empty, Empty)), (Tree(25, Empty, Empty)))), (Tree(36, (Tree(49, Empty, Empty)), (Tree(64, Empty, Empty)))))
    areEqual (treeMap tree (fun x -> x * x)) squaredTree |> should be True

[<Test>]
let ``Should keep the structure``() =
    let checkIfKeepStructure tree =
        let mappedTree = treeMap tree id
        haveSameStructure tree mappedTree
    Check.QuickThrowOnFailure checkIfKeepStructure
