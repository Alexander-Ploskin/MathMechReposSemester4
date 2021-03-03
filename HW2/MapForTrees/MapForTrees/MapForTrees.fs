module MapForTrees

/// <summary>
/// Binaty tree
/// </summary>
type Tree<'a> =
    | Empty
    | Tree of 'a * Tree<'a> * Tree<'a>

/// <summary>
/// Returns new tree with values given by the function on values in nodes of the tree
/// </summary>

let rec treeMap tree func = 
    match tree with 
    | Empty -> Empty
    | Tree(value, left, right) -> Tree(func value, treeMap left func, treeMap right func)
