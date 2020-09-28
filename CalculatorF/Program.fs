namespace CalculatorF

module Calculator =
    open System

    let plus x y = Some(x + y)
    let minus x y = Some(x - y)
    let multiply x y = Some(x * y)
    let divide x y =
        if y = 0.0
        then None
        else Some(x / y)
    
    type MaybeBuilder() =

        member this.Bind(x, f) = 
            match x with
            | None -> None
            | Some a -> f a

        member this.Return(x) = 
            Some x
   
    let maybe = new MaybeBuilder()
     
    let wrapDouble (a:string) =
        let isNumber, value = Double.TryParse(a)
        if isNumber
        then Some(value)
        else None
        
    let possibleOperators = ["+"; "-"; "*"; "/"]
        
    let operatorParse (op:string) = List.exists (fun elem -> elem = op) possibleOperators, op
             
    let calculate x operator y =
        maybe {
            let! result =
                match operator with
                | "+" -> plus x y
                | "-" -> minus x y
                | "*" -> multiply x y
                | "/" -> divide x y
                | _ -> None
            return result
        }   

module Program =
    open System
    open Calculator

    [<EntryPoint>]
    let main argv =
        let x = Console.ReadLine() 
        let op = Console.ReadLine()
        let y = Console.ReadLine()
        
        let isNumber1, val1 = Double.TryParse x
        let isNumber2, val2 = Double.TryParse y
        let isOperator, operator = operatorParse op
        
        if isNumber1 && isNumber2 && isOperator
        then
            let result = calculate val1 operator val2
            if result.IsNone
            then Console.WriteLine("You tried to divide by zero")
            else Console.WriteLine(result)
        else Console.WriteLine("WRONG INPUT: '{0}' or '{1}' is not a number or '{2}' is not a operator", x, y, op)
        0
