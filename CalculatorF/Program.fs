namespace CalculatorF

module Calculator =
    open System

    let plus x y = x + y
    let minus x y = x - y
    let multiply x y = x * y
    let divide x y = x / y

    let calculate x operator y =
        match operator with
        | "+" -> plus x y
        | "-" -> minus x y
        | "*" -> multiply x y
        | "/" -> divide x y
        | _ -> raise (NotSupportedException())


module Program =
    open System
    open Calculator

    [<EntryPoint>]
    let main argv =
        let x = Console.ReadLine() |> double
        let operator = Console.ReadLine()
        let y = Console.ReadLine() |> double
        Console.WriteLine(calculate x operator y)
        0
