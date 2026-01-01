module SettingsPage

open Falco
open Falco.Markup
open Falco.Htmx
open System.Text.Json

let private settingsForm (settings: Map<string, obj>) =
    let defaultTemplate =
        settings.TryFind "defaultFolderNameTemplate"
        |> Option.map string
        |> Option.defaultValue "{YYYY}.{MM}.{DD} {Description}"

    let separateRaw =
        settings.TryFind "separateRawFiles"
        |> Option.map (fun v ->
            match v with
            | :? bool as b -> b
            | :? JsonElement as je -> je.GetBoolean()
            | _ -> false)
        |> Option.defaultValue false

    let rawFolderName =
        settings.TryFind "rawFilesFolderName"
        |> Option.map string
        |> Option.defaultValue "raw"

    Elem.form
        [ Attr.id "settings-form"
          Attr.class' "form-card"
          Hx.put "/settings"
          Attr.create "hx-swap" "none" ]
        [ Elem.div
              [ Attr.class' "field" ]
              [ Elem.label [ Attr.for' "defaultFolderNameTemplate" ] [ Text.raw "Folder Name Template" ]
                Elem.input
                    [ Attr.type' "text"
                      Attr.id "defaultFolderNameTemplate"
                      Attr.name "defaultFolderNameTemplate"
                      Attr.value defaultTemplate ]
                Elem.div [ Attr.class' "hint" ] [ Text.raw "Available: {YYYY}, {MM}, {DD}, {Description}" ] ]
          Elem.div
              [ Attr.class' "field" ]
              [ Elem.div
                    [ Attr.class' "checkbox-field" ]
                    [ Elem.input (
                          [ Attr.type' "checkbox"
                            Attr.id "separateRawFiles"
                            Attr.name "separateRawFiles"
                            Attr.value "true" ]
                          @ (if separateRaw then [ Attr.checked' ] else [])
                      )
                      Elem.div
                          [ Attr.class' "checkbox-label" ]
                          [ Elem.label [ Attr.for' "separateRawFiles" ] [ Text.raw "Separate RAW Files" ]
                            Elem.div [ Attr.class' "hint" ] [ Text.raw "Move RAW files to a separate subfolder" ] ] ]
                Elem.div
                    [ Attr.class' ("nested-field" + if separateRaw then "" else " hidden")
                      Attr.id "raw-folder-field" ]
                    [ Elem.label [ Attr.for' "rawFilesFolderName" ] [ Text.raw "RAW Folder Name" ]
                      Elem.input
                          [ Attr.type' "text"
                            Attr.id "rawFilesFolderName"
                            Attr.name "rawFilesFolderName"
                            Attr.value rawFolderName ] ] ]
          Elem.button [ Attr.type' "submit" ] [ Text.raw "Save Settings" ]
          Elem.script
              []
              [ Text.raw
                    """
                document.getElementById('separateRawFiles').addEventListener('change', function(e) {
                    document.getElementById('raw-folder-field').classList.toggle('hidden', !e.target.checked);
                });
                """ ] ]

let getSettings: HttpHandler =
    fun ctx ->
        let settings = Config.readSettings ()

        let html =
            Layout.page
                "Photo Sync Settings"
                (Layout.header "Settings" (Some "Configure your photo sync preferences")
                 @ [ settingsForm settings; Layout.toastContainer ])

        Response.ofHtml html ctx

let putSettings: HttpHandler =
    fun ctx ->
        task {
            let! form = ctx.Request.ReadFormAsync()

            let defaultTemplate =
                form.TryGetValue "defaultFolderNameTemplate"
                |> function
                    | true, v -> v.ToString()
                    | _ -> "{YYYY}.{MM}.{DD} {Description}"

            let separateRaw =
                form.TryGetValue "separateRawFiles"
                |> function
                    | true, v -> v.ToString() = "true"
                    | _ -> false

            let rawFolderName =
                form.TryGetValue "rawFilesFolderName"
                |> function
                    | true, v -> v.ToString()
                    | _ -> "raw"

            let settings =
                Map
                    [ "defaultFolderNameTemplate", box defaultTemplate
                      "separateRawFiles", box separateRaw
                      "rawFilesFolderName", box rawFolderName ]

            Config.writeSettings settings

            return! Response.ofHtml (Layout.successToast "✓ Settings saved") ctx
        }
