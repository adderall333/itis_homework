// Learn more about F# at http://fsharp.org

open System
open System.Drawing

type Z =
    {
        a: double
        b: double
    }

let sum z1 z2 = { a = z1.a + z2.a; b = z1.b + z2.b }
let abs z = MathF.Sqrt((float32)(z.a * z.a + z.b * z.b))
let square z = { a = z.a * z.a - z.b * z.b; b = 2.0 * z.a * z.b } 

let f z c = sum (square z) c

let func c =
    let mutable z = { a = 0.0; b = 0.0 }
    let mutable i = 0
    while i < 6 && abs z < (float32)2 do
        z <- f z c
        i <- i + 1
    i

[<EntryPoint>]
let main argv =
    let z1 = {a=1.0; b=2.0}
    let z2 = {a=2.0; b=3.0}
    Console.WriteLine(sum z1 z2)
    Console.WriteLine(abs z2)
    for i in 1 .. 20 do
        Console.WriteLine(abs {a=(double)i/4.0; b=(double)i/4.0})
    0 // return an integer exit code

