namespace Shared

open System

type ChocolateBar =
       { Name      : string
         Footprint : Footprint
         Price     : float
         Impact    : Impact option
         TruePrice : float option }
and Footprint =
       { CO2 : float
         CH4 : float }
and Impact =
       { ClimateChange : float }

module ChocolateBarValidation =
    let isValidValue (value : float) = value >= 0.0
    let isValidName (name : string) = String.IsNullOrWhiteSpace name |> not

    let isValidChocolateBar (inputChocolateBar : ChocolateBar) =
    
        //return bool value indicating whether input data is valid
        (inputChocolateBar.Name |> isValidName) &&
        (inputChocolateBar.Footprint.CO2 |> isValidValue) &&
        (inputChocolateBar.Footprint.CH4 |> isValidValue) &&
        (inputChocolateBar.Price |> isValidValue) &&
        (inputChocolateBar.Impact |> Option.isNone) &&
        (inputChocolateBar.TruePrice |> Option.isNone)

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IChocolateBarApi =
    {
        calculateTruePrice : ChocolateBar -> Async<ChocolateBar>
    }