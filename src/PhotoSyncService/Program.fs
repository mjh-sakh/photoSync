open Falco
open Falco.Routing
open Microsoft.AspNetCore.Builder

let _ = Config.readSettings ()

let endpoints =
    [ get "/" SettingsPage.getSettings
      get "/settings" SettingsPage.getSettings
      put "/settings" SettingsPage.putSettings ]

let wapp = WebApplication.Create()

wapp.UseStaticFiles().UseRouting().UseFalco(endpoints).Run(Response.ofPlainText "Not found")
|> ignore

wapp.Run()
