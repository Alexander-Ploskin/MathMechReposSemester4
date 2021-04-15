module LambdaInterpreter

open System

/// Implementation of lambda term by the defenition using type a as an alphabet
type LambdaTerm<'a> =
    | Variable of 'a
    | Application of LambdaTerm<'a> * LambdaTerm<'a>
    | Abstraction of 'a * LambdaTerm<'a>

/// Returns set of free variables in the term
let rec getFreeVariables term =
    let rec getFreeVariablesSubfunction term acc =
        match term with
        | Variable name -> acc |> Set.add name
        | Application(left, right) -> getFreeVariablesSubfunction left acc + getFreeVariablesSubfunction right acc
        | Abstraction(variable, innerTerm) -> getFreeVariablesSubfunction innerTerm acc - set[variable]
    getFreeVariablesSubfunction term Set.empty

/// Returns new value which is not contained in the set
let rec getNewValueNotFromTheSet set =
    let newValue = Guid.NewGuid()
    if set |> Set.contains newValue then getNewValueNotFromTheSet set else newValue

/// Substitutes the variable in the term with the new term
let rec substitute term variableToChange substitutedTerm =
    match term with
    | Variable name when name = variableToChange -> substitutedTerm
    | Variable _ -> term
    | Application(left, right) -> Application(substitute left variableToChange substitutedTerm, substitute right variableToChange substitutedTerm)
    | Abstraction(variable, innerTerm) ->
        match variable with
        | name when name = variableToChange -> term
        | _ when getFreeVariables innerTerm |> Set.contains variableToChange |> not || getFreeVariables substitutedTerm |> Set.contains variable|> not
            -> Abstraction(variable, substitute innerTerm variableToChange substitutedTerm)
        | _ -> let newVariable = getFreeVariables innerTerm + getFreeVariables substitutedTerm |> getNewValueNotFromTheSet
               Abstraction(newVariable, innerTerm |> substitute (Variable newVariable) variable |> substitute substitutedTerm variableToChange)

/// Applies beta-reduction by the normal strategy to the term
/// <param name="term">Lambda term which should use Guid as an alphabet</param>
let rec reduce term = 
    match term with
    | Variable _ -> term
    | Application(left, right) ->
        match left with
        | Abstraction(variable, innerTerm) -> substitute innerTerm variable right |> reduce
        | _ -> Application(reduce left, reduce right)
    | Abstraction(variable, innerTerm) -> Abstraction(variable, reduce innerTerm)
