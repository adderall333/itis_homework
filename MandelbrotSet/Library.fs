module MandelbrotSet

open System.Drawing

type Z =
    {
        a: double
        b: double
    }

let sum z1 z2 = { a = z1.a + z2.a; b = z1.b + z2.b }
let squaredAbs z = z.a * z.a + z.b * z.b
let square z = { a = z.a * z.a - z.b * z.b; b = 2.0 * z.a * z.b } 

let f z c = sum (square z) c

let accuracy = 100

let check c =
    let mutable z = { a = 0.0; b = 0.0 }
    let mutable i = 0
    while i < accuracy && squaredAbs z < 4.0 do
        z <- f z c
        i <- i + 1
    i
        
let getColor z =
    let i = check z
    if i = accuracy
    then Color.Black
    else
    let r = (4 * i) % 255
    let g = (6 * i) % 255
    let b = (8 * i) % 255
    Color.FromArgb(r,g,b)

