module LambdaInterpreter

open System

/// Implementation of lambda term by the defenition
type LambdaTerm<'a> = 
    | Variable of 'a
    | Application of LambdaTerm<'a> * LambdaTerm<'a>
    | Abstraction of 'a * LambdaTerm<'a>

/// Returns set of variables which are free in the term
let rec getFreeVariables term = 
    let rec getFreeVariablesSubFunction term acc =
        match term with 
        | Variable name -> Set.add name acc
        | Application (left, right) -> getFreeVariablesSubFunction left acc + getFreeVariablesSubFunction  right acc
        | Abstraction (variable, term) -> getFreeVariablesSubFunction term acc - set[variable]
    getFreeVariablesSubFunction term Set.empty

/// Generates new value which is not contained in the set
let rec getValueNotContainedInSet set = 
    match Guid.NewGuid() with
    | name when set |> Set.contains name -> getValueNotContainedInSet set
    | name -> name

/// Substitutes varToSubstitute in the term by the newTerm
let rec substitute term varToSubstitute newTerm =
    match term with
    | Variable name when name = varToSubstitute -> newTerm
    | Variable _ -> term
    | Application (left, right) -> Application (substitute left varToSubstitute newTerm, substitute right varToSubstitute newTerm)
    | Abstraction (variable, innerTerm) ->
        match variable with
        | varToSubstitute -> term
        | _ when set[variable; varToSubstitute] |> Set.isSubset (getFreeVariables term) |> not 
            -> Abstraction (variable, substitute innerTerm varToSubstitute newTerm)
        | _ -> let newVariable = getFreeVariables innerTerm + getFreeVariables newTerm |> getValueNotContainedInSet
               Abstraction (newVariable, innerTerm |> substitute (Variable newVariable) variable |> substitute newTerm varToSubstitute)

/// Applies beta reduction to the term by the normal strategy
let rec reduce term = 
    match term with 
    | Variable _ -> term
    | Application (left, right) -> 
        match left with
        | Abstraction (variable, innerTerm) -> substitute innerTerm variable right |> reduce
        | _ -> Application (reduce left, reduce right)
    | Abstraction (variable, abstractionTerm) -> Abstraction (variable, reduce abstractionTerm)
