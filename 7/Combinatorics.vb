Imports System
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace AOC2019.Combinatorics
  Public Module Combinatorics
    Public Function GetPermutations(Of T)(sequence As IEnumerable(Of T)) As IEnumerable(Of IEnumerable(Of T))
      Dim permutations = New List(Of IEnumerable(Of T))

      If sequence.Any Then
        If sequence.Count > 1 Then
          For Each element In sequence
            Dim remaining = sequence.ToList
            remaining.Remove(element)
            For Each remainingPermutation In GetPermutations(remaining)
              Dim permutation = remainingPermutation.ToList
              permutation.Insert(0, element)
              If Not permutations.Any(Function(p) p.SequenceEqual(permutation)) Then
                permutations.Add(permutation)
              End If
            Next
          Next
        Else
          permutations.Add(sequence)
        End If
      End If

      Return permutations
    End Function
  End Module
End Namespace
