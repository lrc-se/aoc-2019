Option Explicit

Dim dicOrbits

' example 1
Set dicOrbits = LoadOrbits("example1.txt")
MsgBox "Number of orbits equals 42: " & (CountAllOrbits(dicOrbits) = 42), , "Example 1"

' puzzle 1
Set dicOrbits = LoadOrbits("input.txt")
MsgBox "Number of orbits: " & CountAllOrbits(dicOrbits), , "Puzzle 1"

' example 2
Set dicOrbits = LoadOrbits("example2.txt")
MsgBox "Number of transfers equals 4: " & (CountOrbitalTransfers(dicOrbits, "YOU", "SAN") = 4), , "Example 2"
MsgBox "Number of transfers equals 6: " & (CountOrbitalTransfers(dicOrbits, "L", "H") = 6), , "Example 2 (extra test)"

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

Function GetOrbitalSeries(Orbits, StartObject)
  Dim strSeries : strSeries = ""
  Dim strCurObject : strCurObject = Orbits.Item(StartObject)
  Do While strCurObject <> ""
    strSeries = strCurObject & ")" & strSeries
    strCurObject = Orbits.Item(strCurObject)
  Loop

  If strSeries <> "" Then
    GetOrbitalSeries = Split(Left(strSeries, Len(strSeries) - 1), ")")
  Else
    GetOrbitalSeries = Array()
  End If
End Function

Function CountOrbitalTransfers(Orbits, StartObject, EndObject)
  ' get orbital series for both objects
  Dim arrStartSeries : arrStartSeries = GetOrbitalSeries(Orbits, StartObject)
  Dim arrEndSeries : arrEndSeries = GetOrbitalSeries(Orbits, EndObject)

  ' sanity checks
  If UBound(arrStartSeries) = -1 Or UBound(arrEndSeries) = -1 Then
    CountOrbitalTransfers = -1
    Exit Function
  End If

  ' find intersection
  Dim i : i = -1
  Do
    If i + 1 > UBound(arrStartSeries) Or i + 1 > UBound(arrEndSeries) Then
      Exit Do
    ElseIf arrStartSeries(i + 1) <> arrEndSeries(i + 1) Then
      Exit Do
    End If
    i = i + 1
  Loop

  ' calculate transfer count
  CountOrbitalTransfers = UBound(arrStartSeries) + UBound(arrEndSeries) - 2 * i
End Function
