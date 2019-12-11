DECLARE FUNCTION CountVisibleObjects% (source AS ANY, map() AS STRING, asteroids() AS ANY)
DECLARE FUNCTION FindBestMonitorIndex% (map() AS STRING, asteroids() AS ANY)
DECLARE FUNCTION IsVisible% (source AS ANY, target AS ANY, map() AS STRING)
DECLARE SUB LoadMap (filename AS STRING, map() AS STRING, asteroids() AS ANY)
DECLARE FUNCTION GCD% (a AS INTEGER, b AS INTEGER)

CONST FALSE = 0
CONST TRUE = NOT FALSE

TYPE Coordinate
  x AS INTEGER
  y AS INTEGER
END TYPE

REDIM map(1) AS STRING
REDIM asteroids(1) AS Coordinate
LoadMap "example1.txt", map(), asteroids()

CLS
PRINT "EXAMPLE 1"
PRINT "========="
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT

PRINT "EXAMPLE 2"
PRINT "========="
LoadMap "example2.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT

PRINT "EXAMPLE 3"
PRINT "========="
LoadMap "example3.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT

PRINT "EXAMPLE 4"
PRINT "========="
LoadMap "example4.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT

PRINT "EXAMPLE 5"
PRINT "========="
LoadMap "example5.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT

PRINT "PUZZLE 1"
PRINT "========"
LoadMap "input.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT

FUNCTION CountVisibleObjects% (source AS Coordinate, map() AS STRING, asteroids() AS Coordinate)
  count% = 0
  FOR i% = 0 TO UBOUND(asteroids)
    IF asteroids(i%).x <> source.x OR asteroids(i%).y <> source.y THEN
      IF IsVisible(source, asteroids(i%), map()) = TRUE THEN
        count% = count% + 1
      END IF
    END IF
  NEXT i%
  CountVisibleObjects% = count%
END FUNCTION

FUNCTION FindBestMonitorIndex% (map() AS STRING, asteroids() AS Coordinate)
  highest% = -1
  index% = -1
  FOR i% = 0 TO UBOUND(asteroids)
    count% = CountVisibleObjects(asteroids(i%), map(), asteroids())
    IF count% > highest% THEN
      highest% = count%
      index% = i%
    END IF
  NEXT i%
  FindBestMonitorIndex% = index%
END FUNCTION

FUNCTION GCD% (a AS INTEGER, b AS INTEGER)
  IF b% = 0 THEN
    GCD% = a%
  ELSE
    GCD% = GCD(b%, a% MOD b%)
  END IF
END FUNCTION

FUNCTION IsVisible% (source AS Coordinate, target AS Coordinate, map() AS STRING)
  diffx% = target.x - source.x
  diffy% = target.y - source.y
  deltax% = ABS(diffx%)
  deltay% = ABS(diffy%)

  ' find step sizes
  IF deltax% <> deltay% AND deltax% > 0 AND deltay% > 0 THEN
    divisor% = GCD(deltax%, deltay%)
    deltax% = deltax% / divisor%
    deltay% = deltay% / divisor%
  ELSE
    IF deltax% > 0 THEN deltax% = 1
    IF deltay% > 0 THEN deltay% = 1
  END IF
  IF diffx% < 0 THEN deltax% = -deltax%
  IF diffy% < 0 THEN deltay% = -deltay%

  ' check occlusions along path
  x% = source.x + deltax%
  y% = source.y + deltay%
  DO
    IF x% = target.x AND y% = target.y THEN
      EXIT DO
    ELSEIF MID$(map(y%), x% + 1, 1) = "#" THEN
      IsVisible% = FALSE
      EXIT FUNCTION
    END IF
    x% = x% + deltax%
    y% = y% + deltay%
  LOOP
  IsVisible% = TRUE
END FUNCTION

SUB LoadMap (filename AS STRING, map() AS STRING, asteroids() AS Coordinate)
  w% = 0
  h% = 0

  ' read map
  OPEN filename FOR INPUT AS #1
  LINE INPUT #1, line$
  IF EOF(1) THEN
    ' LF
    offset% = 1
    FOR i% = 1 TO LEN(line$)
      IF MID$(line$, i%, 1) = CHR$(10) THEN
        IF w% = 0 THEN w% = i% - 1
        REDIM PRESERVE map(h%) AS STRING
        map(h%) = MID$(line$, offset%, w%)
        offset% = offset% + w% + 1
        h% = h% + 1
      END IF
    NEXT i%
  ELSE
    ' CRLF
    w% = LEN(line$)
    DO
      REDIM PRESERVE map(h%) AS STRING
      map(h%) = line$
      h% = h% + 1
      LINE INPUT #1, line$
    LOOP UNTIL EOF(1)
    REDIM PRESERVE map(h%) AS STRING
    map(h%) = line$
    h% = h% + 1
  END IF
  CLOSE #1

  ' find asteroids
  FOR y% = 0 TO h% - 1
    FOR x% = 0 TO w% - 1
      IF MID$(map(y%), x% + 1, 1) = "#" THEN
        REDIM PRESERVE asteroids(index%) AS Coordinate
        asteroids(index%).x = x%
        asteroids(index%).y = y%
        index% = index% + 1
      END IF
    NEXT x%
  NEXT y%
END SUB

