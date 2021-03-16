module PhoneBookTests

open System
open NUnit.Framework
open FsUnit
open System.IO

open Data

[<TestFixture>]
type PhoneBookTests() =
    static member invalidPhoneNumbers = [|
        "+7829333444458768768"
        "+7926123456b"
        "+892933344448798798789"
        "8f994324444"
        "(723)"
        ")732(3243244"
    |]

    static member validPhoneNumbers = [|
        "+79261234567"
        "89261234567"
        "79261234567"
        "+7 926 123 45 67"
        "8(926)123-45-67"
        "9261234567"
        "79261234567"
        "89261234567"
        "8-926-123-45-67"
        "8 927 1234 234"
        "8 927 12 12 888"
        "8 927 12 555 12"
        "8 927 123 8 123"
        "(495)1234567"
        "(495) 123 45 67"
    |]

    member  this.workingDirectory = "../../../../DBs"

    [<TestCaseSource(nameof PhoneBookTests.invalidPhoneNumbers)>]
    member this.``Shouldn't add notes with invalid phone numbers`` (number) =
        (fun () -> insertNote Set.empty "Name" number |> ignore) |> should throw typeof<Exception>

    [<TestCaseSource(nameof PhoneBookTests.validPhoneNumbers)>]
    member this.``Should add notes with valid phone numbers`` (number) =
        insertNote Set.empty "Name" number |> Set.contains ("Name", number) |> should be True

    [<Test>]
    member this.``Should select numbers which belongs to name correctly``() =
        (selectPhoneNumbers (set[("a", "1"); ("a", "2"); ("b", "3")]) "a") |> should equal (set["1"; "2"])

    [<Test>]
    member this.``Should select owners of number correctly``() =
        (selectOwners (set[("a", "2"); ("a", "1"); ("b", "1"); ("c", "4")]) "1") |> should equal (set["a"; "b"])

    [<Test>]
    member this.``Should read from empty file correctly``() =
        (readNotesFromFile Set.empty (this.workingDirectory + "/empty.txt")) |> should equal Set.empty

    [<Test>]
    member this.``Reading from not exist file should fail``() =
        (fun () -> readNotesFromFile Set.empty (this.workingDirectory + "/notexist.txt") |> ignore) |> should throw typeof<Exception>

    [<Test>]
    member this.``Should read from valid file correctly``() =
        (readNotesFromFile Set.empty (this.workingDirectory + "/correctDB.txt")) |> should equal (set[("A", "+79261234567"); ("I", "79261234567")])

    [<Test>]
    member this.``Should read from valid file with dublicates correctly``() =
        (readNotesFromFile Set.empty (this.workingDirectory + "/db_with_dublicates.txt")) |> should equal (set[("A", "+79261234567"); ("I", "79261234567")])

    [<Test>]
    member this.``Should write to file correctly``() =
        let notes = set[("a", "1"); ("a", "2"); ("b", "3")]
        let path = this.workingDirectory + "/testDB.txt"
        writeNotesIntoFile notes path
        let read = readNotesFromFile Set.empty path
        File.WriteAllText(path, String.Empty)
        read |> should equal notes
