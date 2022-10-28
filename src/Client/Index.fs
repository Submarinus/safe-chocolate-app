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
    | SetInputValue value -> { state with InputValue = value }
    | AddChocolateBar ->
        let chocolateBar =
            ChocolateBar.create
                state.InputName
                state.InputValue

        { state with
            InputName = ""
            InputValue = 0.0
            ChocolateBars = 
                printfn "Chocolate bar added"
                List.append state.ChocolateBars [chocolateBar]
            }
    | AddedChocolateBar chocolateBar -> { state with ChocolateBars = state.ChocolateBars @ [ chocolateBar ] }

open Feliz
open Feliz.Bulma

// Helper function to easily construct div with only classes and children
let div (classes: string list) (children: ReactElement list) =
    Html.div [
        prop.classes classes
        prop.children children
    ]

let inputName (state: State) (dispatch: Msg -> unit) =
  Html.div [
    prop.classes [ "field"; "has-addons" ]
    prop.children [
      Html.div [
        prop.classes [ "control"; "is-expanded"]
        prop.children [
          Html.input [
            prop.classes [ "input"; "is-medium" ]
            prop.valueOrDefault state.InputName
            prop.placeholder "What is the name of the chocolate bar?"
            prop.onChange (SetInputName >> dispatch)
          ]
        ]
      ]
    ]
  ]

let inputPrice (state: State) (dispatch: Msg -> unit) =
  Html.div [
    prop.classes [ "field"; "has-addons" ]
    prop.children [
      Html.div [
        prop.classes [ "control"; "is-expanded"]
        prop.children [
          Html.input [
            prop.classes [ "input"; "is-medium"; "number" ]
            prop.type'.number
            prop.valueOrDefault state.InputValue
            prop.placeholder "What is the price of the chocolate bar?"
            prop.onChange (SetInputValue >> dispatch)
          ]
        ]
      ]
    ]
  ]

let createChocolateBar (state : State) (dispatch : Msg -> unit) =
      Html.div [
        prop.className "control"
        prop.children [
          Html.button [
            prop.classes [ "button"; "is-primary"; "is-medium" ]
            prop.onClick (fun _ -> dispatch AddChocolateBar)
            prop.children [
              Html.i [ prop.classes [ "fa"; "fa-plus" ] ]
            ]
          ]
        ]
      ]

let renderChocolateBar (chocolateBar : ChocolateBar) (dispatch: Msg -> unit) =
    div [ "box" ] [
        div [ "columns"; "is-mobile"; "is-vcentered" ] [
        div [ "column" ] [
            Html.p [
            prop.className "Name"
            prop.text chocolateBar.Name
            ]
        ]
        div [ "column" ] [
            Html.p [
            prop.className "Price"
            prop.text chocolateBar.Price
            ]
        ]
        ]
    ]

let chocolateBarLegend =
    div [ "box" ] [
        div [ "columns"; "is-mobile"; "is-vcentered" ] [
            div [ "column" ] [
                Html.p [
                prop.text "Name"
                ]
            ]
            div [ "column" ] [
                Html.p [
                prop.text "Price"
                ]
            ]
        ]
    ]

let chocolateBars (state: State) (dispatch: Msg -> unit) =
  Html.ul [
    for chocolateBar in state.ChocolateBars ->
    Html.ul [
        renderChocolateBar chocolateBar dispatch
        ]
    ]

let appTitle =
  Html.p [
    prop.className "title"
    prop.text "Chocolate Bar App"
  ]

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [
    prop.style [ style.padding 20 ]
    prop.children [
      appTitle
      inputName state dispatch
      inputPrice state dispatch
      createChocolateBar state dispatch
      chocolateBarLegend
      chocolateBars state dispatch
    ]
  ]