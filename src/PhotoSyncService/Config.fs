module Config

open System.IO
open System.Text.Json

let userDataPath =
    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "userData")

let settingsFilePath = Path.Combine(userDataPath, "settings.json")

type SettingsForm =
    { DefaultFolderNameTemplate: string
      SeparateRawFiles: bool
      RawFilesFolderName: string }

let defaultSettings =
    Map
        [ "defaultFolderNameTemplate", box "{YYYY}.{MM}.{DD} {Description}"
          "separateRawFiles", box false
          "rawFilesFolderName", box "raw" ]

let readSettings () =
    printfn "Starting initialization..."

    if not (Directory.Exists userDataPath) then
        Directory.CreateDirectory userDataPath |> ignore
        printfn "Created userData directory at: %s" userDataPath


    let settings =
        if not (File.Exists settingsFilePath) then
            File.WriteAllText(settingsFilePath, JsonSerializer.Serialize defaultSettings)
            defaultSettings
        else
            settingsFilePath
            |> File.ReadAllText
            |> JsonSerializer.Deserialize<Map<string, obj>>
            |> fun loaded -> loaded |> Map.fold (fun acc k v -> Map.add k v acc) defaultSettings

    settings

let writeSettings (settings: Map<string, obj>) =
    File.WriteAllText(settingsFilePath, JsonSerializer.Serialize settings)
