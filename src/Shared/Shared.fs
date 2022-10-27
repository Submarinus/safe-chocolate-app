namespace Shared

open System

type ChocolateBar =
       {
        Name      : string
        Price     : float
       }

module ChocolateBar =
    let isValidValue (value : float) = value >= 0.0
    let isValidName (name : string) = String.IsNullOrWhiteSpace name |> not

    let isValidChocolateBar (inputChocolateBar : ChocolateBar) =

        //return bool value indicating whether input data is valid
        (inputChocolateBar.Name |> isValidName) &&
        (inputChocolateBar.Price |> isValidValue)
    
    let create (name: string) (price : float) =
        {
            Name = name
            Price = price
        }
    
    let createEmpty() = {
        Name = ""
        Price = 0.0
    }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IChocolateBarApi =
    {
        getChocolateBars: unit -> Async<ChocolateBar list>
        addChocolateBar: ChocolateBar -> Async<ChocolateBar>
    }