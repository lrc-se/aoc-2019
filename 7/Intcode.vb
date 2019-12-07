Imports System
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic

Namespace AOC2019
  Public Class IntcodeRunner
    Private Shared ReadOnly Operations As IEnumerable(Of Operation) = New List(Of Operation) From {
      New Operation(1, 3),
      New Operation(2, 3),
      New Operation(3, 1),
      New Operation(4, 1),
      New Operation(5, 2),
      New Operation(6, 2),
      New Operation(7, 3),
      New Operation(8, 3),
      New Operation(99, 0)
    }

    Private InputQueue As Queue(Of Integer) = New Queue(Of Integer)
    Private OutputList As IList(Of Integer) = New List(Of Integer)

    Public Property Program As IList(Of Integer)
    Public Property InputMode As IOMode
    Public Property OutputMode As IOMode
    Public Property Input As Queue(Of Integer)
      Get
        Return If(InputMode = IOMode.Internal, InputQueue, Nothing)
      End Get
      Set
        If InputMode = IOMode.Internal Then
          InputQueue = value
        Else
          Throw New Exception("Cannot set input data when not in internal input mode")
        End If
      End Set
    End Property
    Public ReadOnly Property Output As IEnumerable(Of Integer)
      Get
        Return If(OutputMode = IOMode.Internal, OutputList.AsEnumerable, Nothing)
      End Get
    End Property


    Public Sub New(Optional program As IEnumerable(Of Integer) = Nothing, Optional inputMode As IOMode = IOMode.External, Optional outputMode As IOMode = IOMode.External)
      Me.Program = If(Not program Is Nothing, program.ToList, New List(Of Integer))
      Me.InputMode = inputMode
      Me.OutputMode = outputMode
    End Sub

    Public Sub Run()
      Dim pointer As Integer = 0
      Do While pointer < Program.Count
        Dim instruction = New Instruction(Program(pointer))

        Dim operation = Operations.FirstOrDefault(Function(o) o.Code = instruction.Opcode)
        If operation Is Nothing Then
          Throw New Exception("Unsupported opcode " & instruction.Opcode & " at position " & pointer)
        End If

        Dim parameters = Program.Skip(pointer + 1).Take(operation.ParameterCount).Select(Function(p, i) New Parameter(
          p,
          instruction.ParameterModes.ElementAtOrDefault(i)
        )).ToList

        Select Case operation.Code
          Case 1
            Program(parameters(2).Value) = GetParameterValue(parameters(0)) + GetParameterValue(parameters(1))

          Case 2
            Program(parameters(2).Value) = GetParameterValue(parameters(0)) * GetParameterValue(parameters(1))

          Case 3
            Dim value As Integer
            If InputMode = IOMode.External Then
              Console.Write("Input: ")
              value = Convert.ToInt32(Console.ReadLine)
            Else
              If InputQueue.Any Then
                value = InputQueue.Dequeue()
              Else
                Throw New Exception("Empty input queue for input operation at position " & pointer)
              End If
            End If
            Program(parameters(0).Value) = value

          Case 4
            Dim value As Integer = GetParameterValue(parameters(0))
            If OutputMode = IOMode.External Then
              Console.WriteLine(value)
            Else
              OutputList.Add(value)
            End If

          Case 5
            If GetParameterValue(parameters(0)) <> 0 Then
              pointer = GetParameterValue(parameters(1))
              Continue Do
            End If

          Case 6
            If GetParameterValue(parameters(0)) = 0 Then
              pointer = GetParameterValue(parameters(1))
              Continue Do
            End If

          Case 7
            Program(parameters(2).Value) = If(GetParameterValue(parameters(0)) < GetParameterValue(parameters(1)), 1, 0)

          Case 8
            Program(parameters(2).Value) = If(GetParameterValue(parameters(0)) = GetParameterValue(parameters(1)), 1, 0)

          case 99
            Return
        End Select

        pointer = pointer + operation.ParameterCount + 1
      Loop
    End Sub

    Public Sub ClearOutput()
      If OutputMode = IOMode.Internal Then
        OutputList.Clear
      Else
        Throw New Exception("Cannot clear output data when not in internal output mode")
      End If
    End Sub


    Private Function GetParameterValue(parameter As Parameter) As Integer
      Return If(parameter.Mode = ParameterMode.Position, Program(parameter.Value), parameter.Value)
    End Function
  End Class

  Public Class Operation
    Public Property Code As Integer
    Public Property ParameterCount As Integer

    Public Sub New(code As Integer, parameterCount As Integer)
      Me.Code = code
      Me.ParameterCount = parameterCount
    End Sub
  End Class

  Public Class Instruction
    Public Property Opcode As Integer
    Public Property ParameterModes As IList(Of ParameterMode)

    Public Sub New(code As Integer)
      Dim modes As Integer = code \ 100
      Opcode = code - modes * 100
      ParameterModes = New List(Of ParameterMode)
      Do While modes >= 1
        ParameterModes.Add(DirectCast(modes - (modes \ 10) * 10, ParameterMode))
        modes = modes \ 10
      Loop
    End Sub
  End Class

  Public Class Parameter
    Public Property Value As Integer
    Public Property Mode As ParameterMode

    Public Sub New(value As Integer, mode As ParameterMode)
      Me.Value = value
      Me.Mode = mode
    End Sub
  End Class

  Public Enum ParameterMode
    Position = 0
    Immediate = 1
  End Enum

  Public Enum IOMode
    External = 0
    Internal = 1
  End Enum
End Namespace
