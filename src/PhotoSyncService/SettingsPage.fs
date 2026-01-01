module SettingsPage

open Falco
open Falco.Markup
open Falco.Htmx

let private settingsForm (settings: Config.Settings) =
    let defaultTemplate = settings.DefaultFolderNameTemplate
    let separateRaw = settings.SeparateRawFiles
    let rawFolderName = settings.RawFilesFolderName

    Elem.form
        [ Attr.id "settings-form"
          Attr.class' "form-card"
          Hx.put "/settings"
          Attr.create "hx-swap" "none" ]
        [ Elem.div
              [ Attr.class' "field" ]
              [ Elem.label
                    [ Attr.for' Config.SettingsField.DefaultFolderNameTemplate ]
                    [ Text.raw "Folder Name Template" ]
                Elem.input
                    [ Attr.type' "text"
                      Attr.id Config.SettingsField.DefaultFolderNameTemplate
                      Attr.name Config.SettingsField.DefaultFolderNameTemplate
                      Attr.value defaultTemplate ]
                Elem.div [ Attr.class' "hint" ] [ Text.raw "Available: {YYYY}, {MM}, {DD}, {Description}" ] ]
          Elem.div
              [ Attr.class' "field" ]
              [ Elem.div
                    [ Attr.class' "checkbox-field" ]
                    [ Elem.input (
                          [ Attr.type' "checkbox"
                            Attr.id Config.SettingsField.SeparateRawFiles
                            Attr.name Config.SettingsField.SeparateRawFiles
                            Attr.value "true"
                            Attr.create
                                "hx-on:change"
                                "document.getElementById('raw-folder-field').classList.toggle('hidden', !this.checked)" ]
                          @ (if separateRaw then [ Attr.checked' ] else [])
                      )
                      Elem.div
                          [ Attr.class' "checkbox-label" ]
                          [ Elem.label
                                [ Attr.for' Config.SettingsField.SeparateRawFiles ]
                                [ Text.raw "Separate RAW Files" ]
                            Elem.div [ Attr.class' "hint" ] [ Text.raw "Move RAW files to a separate subfolder" ] ] ]
                Elem.div
                    [ Attr.class' ("nested-field" + if separateRaw then "" else " hidden")
                      Attr.id "raw-folder-field" ]
                    [ Elem.label [ Attr.for' Config.SettingsField.RawFilesFolderName ] [ Text.raw "RAW Folder Name" ]
                      Elem.input
                          [ Attr.type' "text"
                            Attr.id Config.SettingsField.RawFilesFolderName
                            Attr.name Config.SettingsField.RawFilesFolderName
                            Attr.value rawFolderName ] ] ]
          Elem.button [ Attr.type' "submit" ] [ Text.raw "Save Settings" ] ]

let getSettings: HttpHandler =
    fun ctx ->
        let settings = Config.readSettings ()

        let html =
            Layout.page
                "Photo Sync Settings"
                (Layout.header "Settings" (Some "Configure your photo sync preferences")
                 @ [ settingsForm settings; Layout.toastContainer ])

        Response.ofHtml html ctx

let private parseSettings (f: FormData) : Config.Settings =
    { DefaultFolderNameTemplate = f.GetString Config.SettingsField.DefaultFolderNameTemplate
      SeparateRawFiles = f.GetBoolean Config.SettingsField.SeparateRawFiles
      RawFilesFolderName = f.GetString Config.SettingsField.RawFilesFolderName }

let putSettings: HttpHandler =
    Request.mapForm parseSettings (fun settings ->
        Config.writeSettings settings
        Response.ofHtml (Layout.successToast "✓ Settings saved"))
