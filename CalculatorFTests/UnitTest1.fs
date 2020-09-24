namespace CalculatorF

open System

module CalculatorFTests =

    open NUnit.Framework
    open FsUnit
    open Calculator

    [<TestFixture>]
    
    module UnitTests = 

        let calculateTests =
            [
                TestCaseData(1, "+", 2, ExpectedResult = 3, TestName = "One plus two equals three")
                TestCaseData(2, "-", 1, ExpectedResult = 1, TestName = "Two minus one equals one")
                TestCaseData(2, "*", 3, ExpectedResult = 6, TestName = "Two multiply three equals six")
                TestCaseData(4, "/", 2, ExpectedResult = 2, TestName = "Four divided two equals two")
                TestCaseData(5, "/", 2, ExpectedResult = 2.5, TestName = "Five divided two equals two and half")
                TestCaseData(5, "/", 0, ExpectedResult = infinity, TestName = "Five divided zero equals infinity")
            ]
        
        [<TestCaseSource("calculateTests")>]
        let CalculateTests x operator y = 
            calculate x operator y
            
    type ``wrongInput tests``() =
        
        [<Test>]
        member __.``Operator "#" is not supported``() =
            (fun () -> calculate 5.0 "#" 3.0 |> ignore)
            |> should throw typeof<NotSupportedException>
        