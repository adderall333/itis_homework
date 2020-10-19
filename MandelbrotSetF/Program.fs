module MandelbrotSetF

open System
open System.Drawing
open System.Windows.Forms

let size = 400
let mutable scale = 1.0

let startX = -0.73
let startY = -0.18

let accuracy = 200
    
let squaredAbs a b =
    a * a + b * b
    
let square a b =
    a * a - b * b, 2.0 * a * b
    
let rec getIterationsCount i zA zB cA cB maxIterations =
    if i < maxIterations && squaredAbs zA zB < ((double)4.0) then
        let zSquareA, zSquareB = square zA zB
        getIterationsCount (i + 1) (zSquareA + cA) (zSquareB + cB) cA cB maxIterations
    else
        i
        
let mapPlane inMin inMax outMin outMax x y =
    let a = startX + (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin
    let b = startY + (y - inMin) * (outMax - outMin) / (inMax - inMin) + outMin
    a, b
        
let getColor iterationsCount =
    let r = (4 * iterationsCount) % 255
    let g = (6 * iterationsCount) % 255
    let b = (8 * iterationsCount) % 255
    Color.FromArgb(r, g, b)
    
let getImage startX startY =
    let image = new Bitmap(size, size)
    for x = 0 to image.Width - 1 do
        for y = 0 to image.Height - 1 do
            let a, b = mapPlane ((double)0.0) ((double)size) -scale scale ((double)x) ((double)y)
            let count = getIterationsCount 0 0.0 0.0 a b accuracy
            if count = accuracy then
                image.SetPixel(x, y, Color.Black)
            else
                image.SetPixel(x, y, getColor(count))
    image
        
type BufferedForm () =
    inherit Form ()
    override this.OnLoad (e : EventArgs) =
        this.DoubleBuffered <- true
        
[<EntryPoint>]
let main argv =
    let form = new BufferedForm(MaximizeBox = true, Width = size, Height = size)
    form.Paint.Add(fun e -> e.Graphics.DrawImage(getImage startX startY, 0, 0))  

    async { 
    while true do
      do! Async.Sleep(10)
      form.Invalidate()
      scale <- scale * 0.9
    } |> Async.StartImmediate

    Application.Run form
    0
   