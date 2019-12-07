Imports System
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic

Namespace AOC2019
  Public Class Puzzle
    Public Shared Sub Main()
      Console.WriteLine("OLD TESTS")
      Console.WriteLine("=========")
      RunOldTests
      Console.WriteLine

      Console.WriteLine("OLD EXAMPLES")
      Console.WriteLine("============")
      RunOldExamples
      Console.WriteLine

      Console.WriteLine("NEW TESTS")
      Console.WriteLine("=========")
      RunNewTests
      Console.WriteLine

      Console.WriteLine("NEW EXAMPLES")
      Console.WriteLine("============")
      RunNewExamples
    End Sub

    Public Shared Function RunAmplifiers(program As IEnumerable(Of Integer), phaseSettings As IEnumerable(Of Integer), Optional inputSignal As Long = 0)
      Dim signal As Long = inputSignal
      For Each phaseSetting In phaseSettings
        Dim runner = New IntcodeRunner(program, IOMode.Internal, IOMode.Internal)
        runner.Input.Enqueue(phaseSetting)
        runner.Input.Enqueue(signal)
        runner.Run
        signal = runner.Output(0)
      Next
      Return signal
    End Function

    Public Shared Sub RunOldTests()
      Dim runner = New IntcodeRunner

      ' older tests
      runner.Program = New List(Of Integer) From { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }
      runner.Run
      Console.WriteLine("Test 1: " & runner.Program.SequenceEqual({ 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }))

      runner.Program = New List(Of Integer) From { 1, 0, 0, 0, 99 }
      runner.Run
      Console.WriteLine("Test 2: " & runner.Program.SequenceEqual({ 2, 0, 0, 0, 99 }))

      runner.Program = New List(Of Integer) From { 2, 3, 0, 3, 99 }
      runner.Run
      Console.WriteLine("Test 3: " & runner.Program.SequenceEqual({ 2, 3, 0, 6, 99 }))

      runner.Program = New List(Of Integer) From { 2, 4, 4, 5, 99, 0 }
      runner.Run
      Console.WriteLine("Test 4: " & runner.Program.SequenceEqual({ 2, 4, 4, 5, 99, 9801 }))

      runner.Program = New List(Of Integer) From { 1, 1, 1, 4, 99, 5, 6, 0, 99 }
      runner.Run
      Console.WriteLine("Test 5: " & runner.Program.SequenceEqual({ 30, 1, 1, 4, 2, 5, 6, 0, 99 }))

      ' newer tests
      runner.Program = New List(Of Integer) From { 1002, 4, 3, 4, 33 }
      runner.Run
      Console.WriteLine("Test 6: " & runner.Program.SequenceEqual({ 1002, 4, 3, 4, 99 }))

      runner.Program = New List(Of Integer) From { 1101, 100, -1, 4, 0 }
      runner.Run
      Console.WriteLine("Test 7: " & runner.Program.SequenceEqual({ 1101, 100, -1, 4, 99 }))
    End Sub

    Public Shared Sub RunNewTests()
      Dim signal As Integer
      
      signal = RunAmplifiers(
        { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 },
        { 4, 3, 2, 1, 0 }
      )
      Console.WriteLine("Phase settings 43210 result in max signal 43210: " & (signal = 43210))

      signal = RunAmplifiers(
        { 3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0 },
        { 0, 1, 2, 3, 4 }
      )
      Console.WriteLine("Phase settings 01234 result in max signal 54321: " & (signal = 54321))

      signal = RunAmplifiers(
        { 3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0 },
        { 1, 0, 4, 3, 2 }
      )
      Console.WriteLine("Phase settings 10432 result in max signal 65210: " & (signal = 65210))
    End Sub

    Public Shared Sub RunOldExamples()
      Dim runner = New IntcodeRunner

      Console.WriteLine("Equal to 8?")
      runner.Program = New List(Of Integer) From { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }
      runner.Run

      Console.WriteLine("Less than 8?")
      runner.Program = New List(Of Integer) From { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }
      runner.Run

      Console.WriteLine("Equal to 8?")
      runner.Program = New List(Of Integer) From { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }
      runner.Run

      Console.WriteLine("Less than 8?")
      runner.Program = New List(Of Integer) From { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }
      runner.Run

      Console.WriteLine("Non-zero input?")
      runner.Program = New List(Of Integer) From { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }
      runner.Run

      Console.WriteLine("Non-zero input?")
      runner.Program = New List(Of Integer) From { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }
      runner.Run

      Console.WriteLine("Compare to 8")
      runner.Program = New List(Of Integer) From {
        3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
        1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
        999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99
      }
      runner.Run
    End Sub

    Public Shared Sub RunNewExamples()
      Dim runner = New IntcodeRunner(inputMode:=IOMode.Internal, outputMode:=IOMode.Internal)

      Console.WriteLine("Equal to 8?")
      runner.Program = New List(Of Integer) From { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }
      runner.Input.Enqueue(8)
      runner.Run
      Console.WriteLine("Input 8 outputs 1: " & (runner.Output(0) = 1))

      Console.WriteLine("Less than 8?")
      runner.Program = New List(Of Integer) From { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }
      runner.Input = New Queue(Of Integer)({ 7 })
      runner.Run
      Console.WriteLine("Input 7 outputs 1: " & (runner.Output(1) = 1))

      Console.WriteLine("Equal to 8?")
      runner.Program = New List(Of Integer) From { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }
      runner.Input = New Queue(Of Integer)({ 47 })
      runner.Run
      Console.WriteLine("Input 47 outputs 0: " & (runner.Output(2) = 0))

      Console.WriteLine("Less than 8?")
      runner.Program = New List(Of Integer) From { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }
      runner.Input = New Queue(Of Integer)({ 12 })
      runner.Run
      Console.WriteLine("Input 12 outputs 0: " & (runner.Output(3) = 0))

      Console.WriteLine("Non-zero input?")
      runner.Program = New List(Of Integer) From { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }
      runner.Input = New Queue(Of Integer)({ 0 })
      runner.Run
      Console.WriteLine("Input 0 outputs 0: " & (runner.Output(4) = 0))

      Console.WriteLine("Non-zero input?")
      runner.Program = New List(Of Integer) From { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }
      runner.Input = New Queue(Of Integer)({ -1 })
      runner.Run
      Console.WriteLine("Input -1 outputs 1: " & (runner.Output(5) = 1))

      Console.WriteLine("Compare to 8")
      runner.Program = New List(Of Integer) From {
        3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
        1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
        999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99
      }
      runner.Input = New Queue(Of Integer)({ 0 })
      runner.ClearOutput
      runner.Run
      Console.WriteLine("Input 0 outputs 999: " & (runner.Output(0) = 999))

      Console.WriteLine("Compare to 8")
      runner.Input = New Queue(Of Integer)({ 8 })
      runner.ClearOutput
      runner.Run
      Console.WriteLine("Input 8 outputs 1000: " & (runner.Output(0) = 1000))

      Console.WriteLine("Compare to 8")
      runner.Input = New Queue(Of Integer)({ 9 })
      runner.ClearOutput
      runner.Run
      Console.WriteLine("Input 9 outputs 1001: " & (runner.Output(0) = 1001))
    End Sub
  End Class
End Namespace
