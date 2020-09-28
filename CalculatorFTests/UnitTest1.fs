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
                TestCaseData(1, "+", 2, ExpectedResult = Some(3.0), TestName = "One plus two equals three")
                TestCaseData(2, "-", 1, ExpectedResult = Some(1.0), TestName = "Two minus one equals one")
                TestCaseData(2, "*", 3, ExpectedResult = Some(6.0), TestName = "Two multiply three equals six")
                TestCaseData(4, "/", 2, ExpectedResult = Some(2.0), TestName = "Four divided two equals two")
                TestCaseData(5, "/", 2, ExpectedResult = Some(2.5), TestName = "Five divided two equals two and half")
                TestCaseData(5, "/", 0, ExpectedResult = None, TestName = "Division by zero is an impossible operation")
                TestCaseData(5, "#", 3, ExpectedResult = None, TestName = "Wrong operator")
            ]
        
        [<TestCaseSource("calculateTests")>]
        let CalculateTests x operator y = 
            calculate x operator y
     
    //тест заработает, если мне разрешат выбрасывать исключения на неправильный ввод
    //иначе эта часть программы в целом не тестируема
    
    //type ``wrongInput tests``() =
        
    //    [<Test>]
    //    member __.``Operator "#" is not supported``() =
    //        (fun () -> calculate 5.0 "#" 3.0 |> ignore)
    //        |> should throw typeof<NotSupportedException>
        