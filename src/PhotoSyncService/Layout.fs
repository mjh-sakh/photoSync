module Layout

open Falco.Markup
open Falco.Htmx

/// Creates a full HTML page with the shared layout
let page (title: string) (content: XmlNode list) =
    Elem.html
        [ Attr.lang "en" ]
        [ Elem.head
              []
              [ Elem.meta [ Attr.charset "utf-8" ]
                Elem.meta [ Attr.name "viewport"; Attr.value "width=device-width, initial-scale=1" ]
                Elem.title [] [ Text.raw title ]
                Elem.link [ Attr.rel "stylesheet"; Attr.href "/css/app.css" ]
                Elem.script [ Attr.src HtmxScript.cdnSrc ] [] ]
          Elem.body [] [ Elem.div [ Attr.class' "container" ] content ] ]

/// Creates a page header with title and optional subtitle
let header (title: string) (subtitle: string option) =
    [ Elem.h1 [] [ Text.raw title ] ]
    @ match subtitle with
      | Some s -> [ Elem.p [ Attr.class' "subtitle" ] [ Text.raw s ] ]
      | None -> []

/// Toast container for htmx out-of-band swaps
let toastContainer = Elem.div [ Attr.id "toast-container" ] []

/// Creates a success toast notification (for OOB swap)
let successToast (message: string) =
    Elem.div [ Attr.id "toast-container"; Hx.swapOobOn ] [ Elem.div [ Attr.class' "toast" ] [ Text.raw message ] ]

/// Creates an error toast notification (for OOB swap)
let errorToast (message: string) =
    Elem.div
        [ Attr.id "toast-container"; Hx.swapOobOn ]
        [ Elem.div [ Attr.class' "toast toast-error" ] [ Text.raw message ] ]
