open Falco
open Falco.Routing
open Microsoft.AspNetCore.Builder

// Run first-time initialization if needed
let settings = Config.readSettings ()

let wapp = WebApplication.Create()

wapp.UseRouting().UseFalco([ get "/" (Response.ofPlainText "Hello World!") ]).Run(Response.ofPlainText "Not found")
