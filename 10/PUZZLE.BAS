DECLARE FUNCTION CountVisibleObjects% (source AS ANY, map() AS STRING, asteroids() AS ANY)
DECLARE FUNCTION FindBestMonitorIndex% (map() AS STRING, asteroids() AS ANY)
DECLARE FUNCTION GCD% (a AS INTEGER, b AS INTEGER)
DECLARE FUNCTION IsVisible% (source AS ANY, target AS ANY, map() AS STRING)
DECLARE SUB LoadMap (filename AS STRING, map() AS STRING, asteroids() AS ANY)
DECLARE SUB VaporizeAsteroids (source AS ANY, map() AS STRING, asteroids() AS ANY, vaporized() AS ANY)
DECLARE SUB SortLaserTargets (targets() AS ANY, low AS INTEGER, high AS INTEGER)
DECLARE FUNCTION PartitionLaserTargets% (targets() AS ANY, low AS INTEGER, high AS INTEGER)

CONST FALSE = 0
CONST TRUE = NOT FALSE
CONST PI = 3.14159265358979#

TYPE Coordinate
  x AS INTEGER
  y AS INTEGER
END TYPE

TYPE LaserTarget
  index AS INTEGER
  angle AS DOUBLE
  vaporized AS INTEGER
END TYPE

REDIM map(1) AS STRING
REDIM asteroids(1) AS Coordinate
LoadMap "example1.txt", map(), asteroids()

CLS
PRINT "Which part (1-2)? "
part$ = INPUT$(1)
PRINT
IF part$ = "1" THEN
  GOTO Part1
ELSEIF part$ = "2" THEN
  GOTO Part2
ELSE
  PRINT "Fine, be that way!"
  SYSTEM
END IF

Part1:
PRINT "EXAMPLE 1"
PRINT "========="
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT
SLEEP

PRINT "EXAMPLE 2"
PRINT "========="
LoadMap "example2.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT
SLEEP

PRINT "EXAMPLE 3"
PRINT "========="
LoadMap "example3.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT
SLEEP

PRINT "EXAMPLE 4"
PRINT "========="
LoadMap "example4.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT
SLEEP

PRINT "EXAMPLE 5"
PRINT "========="
LoadMap "example5.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
PRINT
SLEEP

PRINT "PUZZLE 1"
PRINT "========"
LoadMap "input.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
PRINT "Best location: ", asteroids(index%).x; asteroids(index%).y
PRINT "Visible objects: ", CountVisibleObjects(asteroids(index%), map(), asteroids())
SYSTEM

Part2:
PRINT "EXAMPLE 6"
PRINT "========="
LoadMap "example6.txt", map(), asteroids()
DIM laser AS Coordinate
laser.x = 8
laser.y = 3
REDIM vaporized(1) AS Coordinate
VaporizeAsteroids laser, map(), asteroids(), vaporized()
PRINT "Vaporized asteroid "; vaporized(0).x; vaporized(0).y, " should match  8  1"
PRINT "Vaporized asteroid "; vaporized(1).x; vaporized(1).y, " should match  9  0"
PRINT "Vaporized asteroid "; vaporized(2).x; vaporized(2).y, " should match  9  1"
PRINT "Vaporized asteroid "; vaporized(3).x; vaporized(3).y, " should match  10  0"
PRINT "Vaporized asteroid "; vaporized(4).x; vaporized(4).y, " should match  9  2"
PRINT "Vaporized asteroid "; vaporized(5).x; vaporized(5).y, " should match  11  1"
PRINT "Vaporized asteroid "; vaporized(6).x; vaporized(6).y, " should match  12  1"
PRINT "Vaporized asteroid "; vaporized(7).x; vaporized(7).y, " should match  11  2"
PRINT "Vaporized asteroid "; vaporized(8).x; vaporized(8).y, " should match  15  1"
PRINT "Vaporized asteroid "; vaporized(9).x; vaporized(9).y, " should match  12  2"
PRINT "Vaporized asteroid "; vaporized(22).x; vaporized(22).y, " should match  0  1"
PRINT "Vaporized asteroid "; vaporized(34).x; vaporized(34).y, " should match  13  3"
PRINT "Vaporized asteroid "; vaporized(UBOUND(vaporized)).x; vaporized(UBOUND(vaporized)).y,
PRINT " should match  14  3"
PRINT
SLEEP

PRINT "EXAMPLE 7"
PRINT "========="
LoadMap "example5.txt", map(), asteroids()
laser.x = 11
laser.y = 13
VaporizeAsteroids laser, map(), asteroids(), vaporized()
PRINT "1st asteroid vaporized: ", vaporized(0).x; vaporized(0).y
PRINT "2nd asteroid vaporized: ", vaporized(1).x; vaporized(1).y
PRINT "3rd asteroid vaporized: ", vaporized(2).x; vaporized(2).y
PRINT "10th asteroid vaporized: ", vaporized(9).x; vaporized(9).y
PRINT "20th asteroid vaporized: ", vaporized(19).x; vaporized(19).y
PRINT "50th asteroid vaporized: ", vaporized(49).x; vaporized(49).y
PRINT "100th asteroid vaporized: ", vaporized(99).x; vaporized(99).y
PRINT "199th asteroid vaporized: ", vaporized(198).x; vaporized(198).y
PRINT "200th asteroid vaporized: ", vaporized(199).x; vaporized(199).y
PRINT "201st asteroid vaporized: ", vaporized(200).x; vaporized(200).y
PRINT "299th asteroid vaporized: ", vaporized(298).x; vaporized(298).y
PRINT "Last asteroid vaporized: ", vaporized(UBOUND(vaporized)).x; vaporized(UBOUND(vaporized)).y
PRINT
SLEEP

PRINT "PUZZLE 2"
PRINT "========"
LoadMap "input.txt", map(), asteroids()
index% = FindBestMonitorIndex(map(), asteroids())
VaporizeAsteroids asteroids(index%), map(), asteroids(), vaporized()
PRINT "200th asteroid vaporized: ", vaporized(199).x; vaporized(199).y
PRINT "Result: "; vaporized(199).x * 100 + vaporized(199).y

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

FUNCTION PartitionLaserTargets% (targets() AS LaserTarget, low AS INTEGER, high AS INTEGER)
  pivot# = targets(high).angle#
  i% = low
  FOR j% = low TO high
    IF targets(j%).angle < pivot# THEN
      SWAP targets(i%), targets(j%)
      i% = i% + 1
    END IF
  NEXT j%
  SWAP targets(i%), targets(high)
  PartitionLaserTargets% = i%
END FUNCTION

SUB SortLaserTargets (targets() AS LaserTarget, low AS INTEGER, high AS INTEGER)
  IF low < high THEN
    partition% = PartitionLaserTargets(targets(), low, high)
    SortLaserTargets targets(), low, partition% - 1
    SortLaserTargets targets(), partition% + 1, high
  END IF
END SUB

SUB VaporizeAsteroids (source AS Coordinate, map() AS STRING, asteroids() AS Coordinate, vaporized() AS Coordinate)
  ' calculate target angles
  count% = 0
  FOR i% = 0 TO UBOUND(asteroids)
    IF asteroids(i%).x <> source.x OR asteroids(i%).y <> source.y THEN
      diffx% = asteroids(i%).x - source.x
      diffy% = asteroids(i%).y - source.y
      deltax% = ABS(diffx%)
      deltay% = ABS(diffy%)

      IF diffx% >= 0 AND diffy% < 0 THEN
        ' first quadrant
        tangent# = deltax% / deltay%
        offset# = 0
      ELSEIF diffx% > 0 AND diffy% >= 0 THEN
        ' second quadrant
        tangent# = deltay% / deltax%
        offset# = PI / 2
      ELSEIF diffx% <= 0 AND diffy% > 0 THEN
        ' third quadrant
        tangent# = deltax% / deltay%
        offset# = PI
      ELSE
        ' fourth quadrant
        tangent# = deltay% / deltax%
        offset# = PI * 3 / 2
      END IF

      REDIM PRESERVE targets(count%) AS LaserTarget
      targets(count%).index = i%
      targets(count%).angle = offset# + ATN(tangent#)
      targets(count%).vaporized = FALSE
      count% = count% + 1
    END IF
  NEXT i%

  ' sort targets by increasing clockwise angle
  SortLaserTargets targets(), 0, count% - 1

  ' vaporize targets in angle order
  DIM asteroid AS Coordinate
  count2% = 0
  DO
    ' find next asteroid that can be vaporized
    lastangle# = -1
    FOR i% = 0 TO count% - 1
      IF targets(i%).angle <> lastangle# AND targets(i%).vaporized = FALSE THEN
        asteroid = asteroids(targets(i%).index)
        IF IsVisible(source, asteroid, map()) = TRUE THEN
          ' asteroid in range, so add to vaporized list
          REDIM PRESERVE vaporized(count2%) AS Coordinate
          vaporized(count2%).x = asteroid.x
          vaporized(count2%).y = asteroid.y
          targets(i%).vaporized = TRUE
          lastangle# = targets(i%).angle
          count2% = count2% + 1

          ' erase asteroid from map
          MID$(map(asteroid.y), asteroid.x + 1, 1) = "."
        END IF
      END IF
    NEXT i%
  LOOP UNTIL count2% = count%
END SUB

