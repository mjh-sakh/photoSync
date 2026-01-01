module Config

open System.IO
open System.Text.Json
open System.Text.Json.Nodes

let userDataPath =
    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "userData")

let settingsFilePath = Path.Combine(userDataPath, "settings.json")

type Settings =
    { DefaultFolderNameTemplate: string
      SeparateRawFiles: bool
      RawFilesFolderName: string }

module SettingsField =
    [<Literal>]
    let DefaultFolderNameTemplate = "defaultFolderNameTemplate"

    [<Literal>]
    let SeparateRawFiles = "separateRawFiles"

    [<Literal>]
    let RawFilesFolderName = "rawFilesFolderName"

let defaultSettings: Settings =
    { DefaultFolderNameTemplate = "{YYYY}.{MM}.{DD} {Description}"
      SeparateRawFiles = false
      RawFilesFolderName = "raw" }

let readSettings () : Settings =
    printfn "Starting initialization..."

    if not (Directory.Exists userDataPath) then
        Directory.CreateDirectory userDataPath |> ignore
        printfn "Created userData directory at: %s" userDataPath

    if not (File.Exists settingsFilePath) then
        let json = JsonSerializer.Serialize(defaultSettings)
        File.WriteAllText(settingsFilePath, json)
        defaultSettings
    else
        let defaultNode = JsonSerializer.SerializeToNode(defaultSettings).AsObject()
        let userNode = settingsFilePath |> File.ReadAllText |> JsonNode.Parse
        let userProperties = userNode.AsObject()

        userProperties
        |> Seq.iter (fun property -> defaultNode.[property.Key] <- property.Value.DeepClone())

        defaultNode.Deserialize<Settings>()



let writeSettings (settings: Settings) =
    let json = JsonSerializer.Serialize(settings)
    File.WriteAllText(settingsFilePath, json)
