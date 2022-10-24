module Index

open Elmish
open Fable.Remoting.Client
open Shared

type Model = { ChocolateBars: ChocolateBar list; Input: string }

type Msg =
    | GotChocolateBars of ChocolateBar list
    | SetInput of string
    | AddChocolateBar
    | AddedChocolateBar of ChocolateBar

let chocolateBarsApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IChocolateBarApi>

let init () : Model * Cmd<Msg> =
    let model = { ChocolateBars = []; Input = "" }

    let cmd = Cmd.OfAsync.perform chocolateBarsApi.getChocolateBars () GotChocolateBars

    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | GotChocolateBars chocolateBars -> { model with ChocolateBars = chocolateBars }, Cmd.none
    | SetInput value -> { model with Input = value }, Cmd.none
    | AddChocolateBar ->
        let chocolateBar = ChocolateBar.create model.Input

        let cmd = Cmd.OfAsync.perform chocolateBarsApi.addChocolateBar chocolateBar AddedChocolateBar

        { model with Input = "" }, cmd
    | AddedChocolateBar chocolateBar -> { model with ChocolateBars = model.ChocolateBars @ [ chocolateBar ] }, Cmd.none

open Feliz
open Feliz.Bulma

let navBrand =
    Bulma.navbarBrand.div [
        Bulma.navbarItem.a [
            prop.href "https://safe-stack.github.io/"
            navbarItem.isActive
            prop.children [
                Html.img [
                    prop.src "/favicon.png"
                    prop.alt "Logo"
                ]
            ]
        ]
    ]

let containerBox (model: Model) (dispatch: Msg -> unit) =
    Bulma.box [
        Bulma.content [
            Html.ol [
                for chocolateBar in model.ChocolateBars do
                    Html.li [ prop.text chocolateBar.Name ]
            ]
        ]
        Bulma.field.div [
            field.isGrouped
            prop.children [
                Bulma.control.p [
                    control.isExpanded
                    prop.children [
                        Bulma.input.text [
                            prop.value model.Input
                            prop.placeholder "What is the name?"
                            prop.onChange (fun x -> SetInput x |> dispatch)
                        ]
                        // Bulma.input.text [
                        //     prop.value model.Input
                        //     prop.placeholder "What is the CO2 footprint?"
                        //     prop.onChange (fun x -> SetInput x |> dispatch)
                        // ]
                        // Bulma.input.text [
                        //     prop.value model.Input
                        //     prop.placeholder "What is the CH4 footprint?"
                        //     prop.onChange (fun x -> SetInput x |> dispatch)
                        // ]
                    ]
                ]
                Bulma.control.p [
                    Bulma.button.a [
                        color.isPrimary
                        prop.disabled (ChocolateBar.isValidName model.Input |> not)
                        prop.onClick (fun _ -> dispatch AddChocolateBar)
                        prop.text "Add"
                    ]
                ]
            ]
        ]
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Bulma.hero [
        hero.isFullHeight
        color.isPrimary
        prop.style [
            style.backgroundSize "cover"
            style.backgroundImageUrl "https://unsplash.it/1200/900?random"
            style.backgroundPosition "no-repeat center center fixed"
        ]
        prop.children [
            Bulma.heroHead [
                Bulma.navbar [
                    Bulma.container [ navBrand ]
                ]
            ]
            Bulma.heroBody [
                Bulma.container [
                    Bulma.column [
                        column.is6
                        column.isOffset3
                        prop.children [
                            Bulma.title [
                                text.hasTextCentered
                                prop.text "safe_chocolate_app"
                            ]
                            containerBox model dispatch
                        ]
                    ]
                ]
            ]
        ]
    ]