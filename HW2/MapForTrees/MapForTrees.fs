module MapForTrees

// Binary tree
type Tree<'a> =
    | Empty
    | Tree of 'a * Tree<'a> * Tree<'a>

// Returns new tree with values given by the function on values in nodes of the tree
let rec treeMap tree func = 
    match tree with 
    | Empty -> Empty
    | Tree(value, left, right) -> Tree(func value, treeMap left func, treeMap right func)
