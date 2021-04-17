﻿module Data

open System.IO
open System
open System.Text.RegularExpressions

/// Regular expression which matches with russian phone numbers
let numberValidationExpression =
    "^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"

/// Regular expression which matches with paths of .txt files
let pathValidationExpression =
    "^(.+)\/([^\/]+).txt$"

/// Wrapper of Regex function, returns true if string matches with expression
let matchWithExpression expression string =
    Regex.IsMatch(string, expression)

/// Reads notes from the specified file
let readNotesFromFile notes path =
    if path |> matchWithExpression pathValidationExpression |> not then raise <| ArgumentException("Path is not correct")
    try
        File.ReadAllLines(path) |>  Seq.map (fun (string : String) -> string.Split("-")) 
        |> Seq.map (fun (note : string[]) -> note.[0], note.[1]) |> Set.ofSeq |> (+) notes
    with
        | :? DirectoryNotFoundException -> failwith "There is not such directory"
        | :? FileNotFoundException -> failwith "There is not file by this path"
        | :? ArgumentException -> failwith "Path is not correct"

/// Writes notes into the specified file
let writeNotesIntoFile notes path =
    if path |> matchWithExpression pathValidationExpression |> not then raise <| ArgumentException("Path is not correct")
    try
        let serializedNote = Set.map (fun note -> $"{fst note}-{snd note}") notes
        File.AppendAllLines(path, serializedNote)
    with
        | :? FileNotFoundException -> failwith "There is not file by this path"
        | :? ArgumentException -> failwith "Path is not correct"

/// Selects all names with specified phone
let selectOwners (notes : Set<string * string>) (number : string) =
    Set.filter (fun note -> snd note = number) notes |> Set.map fst

/// Selects all phone numbers belong to the specified name
let selectPhoneNumbers (notes : Set<string * string>) name =
    Set.filter (fun note -> fst note = name) notes |> Set.map snd

/// Inserts new note into the set of notes
let insertNote notes name phoneNumber =
    if phoneNumber |> matchWithExpression numberValidationExpression |> not then failwith "Number is not correct"
    notes |> Set.add (name, phoneNumber)
