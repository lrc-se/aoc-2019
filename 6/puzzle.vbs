Option Explicit

Dim dicOrbits

' example 1
Set dicOrbits = LoadOrbits("example1.txt")
MsgBox "Number of orbits equals 42: " & (CountAllOrbits(dicOrbits) = 42), , "Example 1"

Set dicOrbits = Nothing


' functions

Function LoadOrbits(Filename)
  Dim dicOrbits : Set dicOrbits = CreateObject("Scripting.Dictionary")

  Dim objFS : Set objFS = CreateObject("Scripting.FileSystemObject")
  Dim objFile : Set objFile = objFS.OpenTextFile(Filename, 1)
  Do While Not objFile.AtEndOfStream
    Dim arrObjects : arrObjects = Split(objFile.ReadLine, ")")
    dicOrbits.Item(arrObjects(1)) = arrObjects(0)
  Loop
  Set objFile = Nothing
  Set objFS = Nothing

  Set LoadOrbits = dicOrbits
End Function

Function CountOrbits(Orbits, StartObject)
  Dim lngCount : lngCount = 0
  Dim strCurObject : strCurObject = Orbits.Item(StartObject)
  Do While strCurObject <> ""
    lngCount = lngCount + 1
    strCurObject = Orbits.Item(strCurObject)
  Loop
  CountOrbits = lngCount
End Function

Function CountAllOrbits(Orbits)
  Dim lngCount : lngCount = 0
  Dim Obj
  For Each Obj In Orbits
    lngCount = lngCount + CountOrbits(Orbits, Obj)
  Next
  CountAllOrbits = lngCount
End Function
