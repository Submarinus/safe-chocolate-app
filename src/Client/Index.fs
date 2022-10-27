module Index

open Elmish
open Fable.Remoting.Client
open Shared

type State = {
    ChocolateBars : ChocolateBar list
    InputName : string
    InputValue : float
}

type Msg =
    | GotChocolateBars of ChocolateBar list
    | SetInputName of string
    | SetInputValue of float
    | AddChocolateBar
    | AddedChocolateBar of ChocolateBar

let chocolateBarsApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IChocolateBarApi>

let init() = {
    ChocolateBars = [
        {
            Name = "Tony's #1"
            Price = 3.2
            }
        {
            Name = "Tony's #2"
            Price = 3.0
            }
    ]
    InputName = ""
    InputValue = 0.0
    }

let update (msg: Msg) (state: State) : State =
    match msg with
    | GotChocolateBars chocolateBars -> { state with ChocolateBars = chocolateBars }
    | SetInputName name -> { state with InputName = name }
    | SetInputValue value -> { state with InputValue = value}
    | AddChocolateBar ->
        let chocolateBar = ChocolateBar.create
                                            state.InputName
                                            state.InputValue

        { state with
            InputName = ""
            InputValue = 0.0
            ChocolateBars = List.append state.ChocolateBars [chocolateBar]
            }
    | AddedChocolateBar chocolateBar -> { state with ChocolateBars = state.ChocolateBars @ [ chocolateBar ] }

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

let containerBox (state : State) (dispatch: Msg -> unit) =
    Bulma.box [
        Bulma.content [
                for chocolateBar in state.ChocolateBars do
                    Html.li [
                        Html.li [
                            prop.text chocolateBar.Name
                            prop.value chocolateBar.Price
                        ]
                    ]
        ]
        Bulma.field.div [
            field.isGrouped
            prop.children [
                Bulma.control.p [
                    control.isExpanded
                    prop.children [
                        Bulma.input.text [
                            prop.value state.InputName
                            prop.placeholder "What is the name of the chocolate bar?"
                            prop.onChange (fun x -> SetInputName x |> dispatch)
                        ]
                    ]
                ]
                Bulma.control.p [
                    Bulma.button.a [
                        color.isPrimary
                        prop.disabled (ChocolateBar.isValidName state.InputName |> not)
                        prop.onClick (fun _ -> dispatch AddChocolateBar)
                        prop.text "Add"
                    ]
                ]
            ]
        ]
        Bulma.field.div [
            field.isGrouped
            prop.children [
                Bulma.control.p [
                    control.isExpanded
                    prop.children [
                        Bulma.input.number [
                            prop.value state.InputValue
                            prop.placeholder "What is the price of the chocolate bar?"
                            prop.onChange (fun x -> SetInputValue x |> dispatch)
                        ]
                    ]
                ]
                Bulma.control.p [
                    Bulma.button.a [
                        color.isPrimary
                        prop.disabled (ChocolateBar.isValidValue state.InputValue |> not)
                        prop.onClick (fun _ -> dispatch AddChocolateBar)
                        prop.text "Add"
                    ]
                ]
            ]
        ]
    ]

let view (state : State) (dispatch: Msg -> unit) =
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
                            containerBox state dispatch
                        ]
                    ]
                ]
            ]
        ]
    ]