module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared

module Storage =
    let chocolateBars = ResizeArray()

    let addChocolateBar (chocolateBar: ChocolateBar) =
        if ChocolateBar.isValidChocolateBar(chocolateBar) then
            chocolateBars.Add chocolateBar
            Ok()
        else
            Error "Invalid chocolate bar"

let chocolateBarsApi =
    { getChocolateBars = fun () -> async { return Storage.chocolateBars |> List.ofSeq }
      addChocolateBar =
        fun chocolateBar ->
            async {
                return
                    match Storage.addChocolateBar chocolateBar with
                    | Ok () -> chocolateBar
                    | Error e -> failwith e
            } }

let webApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue chocolateBarsApi
    |> Remoting.buildHttpHandler

let app =
    application {
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }

[<EntryPoint>]
let main _ =
    run app
    0