Advent of Code 2019
===================

Solutions for the 2019 edition of Advent of Code, with each day in a subfolder of its own.
The plan is to use as many languages and platforms/environments as possible, not excluding obsolete ones.
We'll see how it goes.


Day 1
-----

__JavaScript (Node 11.6)__

Quick-and-dirty dependency-less stuff, with equally quick-and-dirty tests (of sorts).


Day 2
-----

__Python (3.8)__

Class-based public-member approach, with unit tests.


Day 3
-----

__PHP (7.3)__

Using various built-in array functions.


Day 4
-----

__Pascal (Turbo Pascal 7)__

A blast from the past! Written and compiled with TPX in dosbox.


Day 5
-----

__C# (.NET 4.7)__

Back to the future. Mmm, types.


Day 6
-----

__VBScript (Windows Scripting Host)__

Without .NET framework dependencies. To run as classic ASP, use `Server.MapPath` for file access and replace the `MsgBox` calls with `Response.Write` (and some line breaks).


Day 7
-----

__VB (.NET 4.7)__

Back in CLR land, now with multithreading.


Day 8
-----

__Assembly (x86)__

Another visit to the distant past, using DOS service calls. Numerical values are printed in big-endian hex format, and in the rendered image `.` represents black and `#` white.
Assembled with NASM to .COM binary targets and tested in dosbox.


Day 9
-----

__Java (JDK 13)__

The intcode runner is ported from the previous .NET version and so may not be entirely idiomatic. No project structure, just plain source files.


Day 10
------

__QuickBASIC (7.1)__

Well, this took some doing (and thinking), but a mathematical approach simplified things.
Some hoops had to be jumped through in order to support both CRLF and LF files,
not to mention the need to implement quicksort from scratch for the second part. Whew!
Written and run with QBX in dosbox.


Day 11
------

__JavaScript (browser/DOM)__

Yet another port of the intcode runner, which is now wholly event-based. Vanilla ES5 all the way, with AJAX retrieval of input data.


Day 12
------

__C# (.NET 4.7)__

First full return show of the series. Still big on types, but with a string-based shortcut in part two.


Day 13
------

__JavaScript (Vue 2.6)__

Using the previous version of the intcode runner as-is, but with Vue handling the frontend this time.
There's no build step so we're still not going beyond ES5, but with a Promise polyfill.

To play, use the arrow keys to advance one frame: `left` to move left, `right` to move right, and `down` to remain in the same position.
In addition, `enter` toggles auto mode and `esc` resets the game. And yes, that's an actual paddle.


Day 16 (part 1)
---------------

__PHP (7.3)__

Arrays again. Mmm, arrays.


Day 17
------

__JavaScript (Node 11.6)__

Using the ASCII intcode runner from day 21. Movement instructions for the second part were arrived at manually (which wasn't very difficult).


Day 19
------

__Java (JDK 13)__

Straight copy of the intcode runner from the previous Java implementation (day 9). Quick math-based solutions, without bells or whistles.


Day 21 (part 1)
---------------

__JavaScript (Node 11.6)__

Back in ES6+ territory with an updated version of the JS intcode runner (day 11+13), utilizing Node's event system.


Day 22 (part 1)
-----------------

__Python (3.8)__

Sequence handling is easy in Python. At least when they're not ginormous...


Day 23
------

__JavaScript (Node 11.6)__

Using the previous intcode runner, sans ASCII extension but with more Node events.


Day 24
------

__C# (.NET 4.7)__

Nothing of particular note in part one, but part two took some thinking to limit recursion levels and handle all tile updates in proper sequence.


Day 25
------

__JavaScript (Node 11.6)__

Using the previous ASCII intcode runner. Enter an empty command to quit the game.
