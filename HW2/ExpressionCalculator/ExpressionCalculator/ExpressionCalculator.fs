module ExpressionCalculator

//A tree of the parsed arithmetical expression
type Expression =
    | Number of int
    | Product of Expression * Expression
    | Quotient of Expression * Expression
    | Sum of Expression * Expression
    | Difference of Expression * Expression

// Calculates parsed arithmetical expression
let rec calculate expression = 
    match expression with
    | Number x -> x
    | Product(x, y) -> calculate x * calculate y
    | Quotient(x, y) -> calculate x / calculate y
    | Sum(x, y) -> calculate x + calculate y
    | Difference(x, y) -> calculate x - calculate y
