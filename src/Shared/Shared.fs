namespace Shared

open System

type ChocolateBar =
       {
        Name      : string
       }

module ChocolateBar =
    let isValidName (name : string) = String.IsNullOrWhiteSpace name |> not
    
    let create (name: string) =
        {
            Name = name
        }
    
    let createEmpty() = {
        Name = ""
    }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IChocolateBarApi =
    {
        getChocolateBars: unit -> Async<ChocolateBar list>
        addChocolateBar: ChocolateBar -> Async<ChocolateBar>
    }