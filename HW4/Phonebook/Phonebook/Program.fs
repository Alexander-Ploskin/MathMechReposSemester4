module PhoneBook

open System
open System.Text.RegularExpressions
open System.IO

/// Prints introduction which contains all necessary commands
let printIntroduction =
    printfn "Welcome to the phonebook app"
    printfn ""
    printfn "Here is the list of instructions to use application\nEnter the number of instruction to execute it"
    printfn "1 - exit"
    printfn "2 - add new note"
    printfn "3 - find the phone number by name"
    printfn "4 - find the name by phone number"
    printfn "5 - print all notes"
    printfn "6 - save data"
    printfn "7 - get data from file"

/// Regular expression which matches with russian phone numbers
let numberValidationExpression =
    "^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"

/// Regular expression which matches with .txt files
let pathValidationExpression =
    "^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.txt$"

/// Wrapper of Regex function, returns true if string matches with expression
let matchWithExpression string expression =
    try
        Regex.IsMatch(string, expression)
    with
        | :? ArgumentException -> false

/// Gets console input of string which should be correct by defenition in isCorrect function
let rec enterCorrect isCorrect =
    let input = Console.ReadLine()
    match input with
    | string when isCorrect string -> input
    | "0" -> input
    | _ -> printfn "Input is not correct try again or enter 0 to leave"
           enterCorrect isCorrect

/// Adds new note with console inputed name and phone number
let addNote notes =
    printfn "Enter the name:"
    let name = Console.ReadLine()
    printfn "Enter the phone number or enter 0 to leave"
    let phoneNumber = enterCorrect (matchWithExpression numberValidationExpression)
    if phoneNumber = "0" then ()
    else printfn "New note saved"
    notes |> Set.add (name, phoneNumber)

/// Finds all numbers which belong to the name
let findNumbers notes =
    printfn "Enter the name:"
    let name = Console.ReadLine()
    let searchResult = Set.filter (fun note -> fst note = name) notes
    printfn $"Numbers of {name}"
    Set.iter (fun note -> printfn "%A" <| snd note) searchResult

/// Finds all owners of the phone number
let findOwners notes =
    printfn "Enter the number:"
    let number = enterCorrect (matchWithExpression numberValidationExpression)
    let searchResult = Set.filter (fun note -> snd note = number) notes
    printfn $"Owners of {number}:"
    Set.iter (fun note -> printfn "%A" <| fst note) searchResult

/// Saves current data into file by the specied path
let saveData notes =
    printfn "Enter the database file path or path to create new file"
    let path = enterCorrect (matchWithExpression pathValidationExpression)
    let bigNote = Set.map (fun note -> $"{fst note}-{snd note}") notes
    try
        File.WriteAllLines(path, bigNote)
        printfn "Saved"
    with
        | :? FileNotFoundException -> printfn "There's not file by this path"
        | :? ArgumentException -> printfn "Path shouldn't be null"
        | :? UnauthorizedAccessException -> printfn "Access denied"

/// Reads data from the file
let readData notes =
    printfn "Enter the database file path"
    let path = enterCorrect (matchWithExpression pathValidationExpression)
    try
        let temp = Seq.map (fun (string : String) -> (Array.get (string.Split("-")) 0, Array.get (string.Split("-")) 1)) (File.ReadAllLines(path))
        printfn "Read"
        Set.ofSeq temp
    with 
        | :? FileNotFoundException  -> printfn "There is not file by this path"
                                       notes
        | :? ArgumentException -> printfn "Path shouldn't be null"
                                  notes

/// Main function of the application
let main =
    let rec loop notes=
        printfn "Enter the number of instruction:"
        match Console.ReadLine() with
        | "1" -> ()
        | "2" -> loop <| addNote notes 
        | "3" -> findNumbers notes
                 loop notes 
        | "4" -> findOwners notes
                 loop notes 
        | "5" -> Set.iter (fun note -> printfn "%A" <| $"{fst note} - {snd note}") notes
                 loop notes 
        | "6" -> saveData notes
                 loop notes
        | "7" -> loop <| (readData notes) + notes
        | _ -> loop notes
    
    printIntroduction
    loop Set.empty

main
