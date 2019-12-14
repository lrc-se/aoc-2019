var Puzzle = (function() {
  "use strict";

  var OutputTypes = {
    X: 0,
    Y: 1,
    TILE: 2
  };
  var width = 100;
  var height = 100;

  var input;
  var screen;
  var pos;
  var outputType;


  function handleOutput(value) {
    switch(outputType) {
      case OutputTypes.X:
        pos.x = value;
        break;
      case OutputTypes.Y:
        pos.y = value;
        break;
      case OutputTypes.TILE:
        screen[pos.y][pos.x] = value;
        break;
    }

    outputType++;
    if(outputType > OutputTypes.TILE) {
      outputType = OutputTypes.X;
    }
  }

  function handleHalt() {
    var numBlocks = 0;
    screen.forEach(function(row) {
      row.forEach(function(tile) {
        if(tile === 2) {
          numBlocks++;
        }
      });
    });
    console.log("Number of block tiles: " + numBlocks);
  }

  function runPuzzle1() {
    reset();
    var runner = Intcode.createRunner(input);
    runner.onoutput = handleOutput;
    runner.onhalt = handleHalt;
    runner.run();
  }

  function reset() {
    screen = [];
    for(var y = 0; y < width; ++y) {
      var row = [];
      for(var x = 0; x < height; ++x) {
        row.push(0);
      }
      screen.push(row);
    }

    outputType = OutputTypes.X;
    pos = {
      x: 0,
      y: 0
    };
  }

  function getJSON(name, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", name + ".json");
    xhr.onreadystatechange = function() {
      if(xhr.readyState == 4) {
        callback(JSON.parse(xhr.responseText));
      }
    };
    xhr.send();
  }

  function init() {
    getJSON("input", function(program) {
      input = program;
      runPuzzle1();
    });
  }

  return {
    init: init
  };
})();
