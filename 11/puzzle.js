var Puzzle = (function(doc) {
  "use strict";

  var Actions = {
    PAINT: 0,
    TURN: 1
  };
  var deltas = {
    1: {
      x: 0,
      y: -1
    },
    2: {
      x: 1,
      y: 0
    },
    3: {
      x: 0,
      y: 1
    },
    4: {
      x: -1,
      y: 0
    }
  };
  var width = 100;
  var height = 100;

  var input;
  var hull;
  var painted;
  var action;
  var pos;
  var direction;


  function handleOutput(runner, value) {
    switch(action) {
      case Actions.PAINT:
        hull[pos.y][pos.x] = value;
        painted[pos.y][pos.x] = true;
        action = Actions.TURN;
        break;

      case Actions.TURN:
        if(value === 0) {
          direction--;
          if(direction < 1) {
            direction = 4;
          }
        } else if(value === 1) {
          direction++;
          if(direction > 4) {
            direction = 1;
          }
        }
        pos.x += deltas[direction].x;
        pos.y += deltas[direction].y;

        action = Actions.PAINT;
        if(AOC.isRunning()) {
          runner.enqueueInput(scanPanel());
        }
        break;
    }
  }

  function handleHalt() {
    AOC.setState("halted");

    hull.forEach(function(row) {
      AOC.printLine(row.map(function(panel) {
        return (panel ? "█" : "·");
      }).join(""));
    });
    AOC.printLine();

    var numPainted = 0;
    painted.forEach(function(row) {
      row.forEach(function(panel) {
        if(panel) {
          numPainted++;
        }
      });
    });
    AOC.printLine("Number of painted panels: " + numPainted);
  }

  function runPuzzle1() {
    if(AOC.isRunning()) {
      alert("Robot already running!")
      return;
    }
    AOC.setState("running");
    reset();

    var runner = Intcode.createRunner(input);
    runner.onoutput = function(value) {
      handleOutput(runner, value);
    };
    runner.onhalt = handleHalt;
    runner.enqueueInput(scanPanel());
    setTimeout(function() {
      runner.run();
    });
  }

  function scanPanel() {
    return hull[pos.y][pos.x];
  }

  function reset() {
    hull = [];
    painted = [];
    for(var y = 0; y < width; ++y) {
      var hullRow = [];
      var paintedRow = [];
      for(var x = 0; x < height; ++x) {
        hullRow.push(0);
        paintedRow.push(false);
      }
      hull.push(hullRow);
      painted.push(paintedRow);
    }

    action = Actions.PAINT;
    pos = {
      x: width / 2,
      y: height / 2
    };
    direction = 1;
    AOC.clearOutput();
  }

  function init() {
    AOC.init();
    AOC.getJSON("input", function(program) {
      input = program;
      doc.getElementById("run-puzzle1").addEventListener("click", runPuzzle1);
    });
  }

  return {
    init: init
  };
})(document);
