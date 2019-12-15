var Puzzle = (function() {
  "use strict";

  var OutputTypes = {
    X: 0,
    Y: 1,
    TILE: 2
  };

  var app;

  function getJSON(name) {
    return new Promise(function(resolve, reject) {
      var xhr = new XMLHttpRequest();
      xhr.open("GET", name + ".json");
      xhr.onreadystatechange = function() {
        if(xhr.readyState == 4) {
          if(xhr.status == 200) {
            resolve(JSON.parse(xhr.responseText));
          } else {
            reject();
          }
        }
      };
      xhr.send();
    });
  }

  function init() {
    var ArcadeScreen = {
      props: {
        width: {
          type: Number,
          required: true
        },

        height: {
          type: Number,
          required: true
        },

        screen: {
          type: Array,
          required: true
        }
      },

      data: function() {
        return {

        };
      }
    };

    var Arcade = {
      components: {
        "aoc-arcade-screen": ArcadeScreen
      },

      props: {
        width: {
          type: Number,
          default: 100
        },

        height: {
          type: Number,
          default: 100
        },

        quarters: {
          type: Number,
          default: 0
        }
      },

      data: function() {
        return {
          program: null,
          finished: false,
          screen: [],
          outputType: null,
          pos: {}
        };
      },

      methods: {
        reset: function() {
          this.screen = [];
          for(var y = 0; y < this.height; ++y) {
            var row = [];
            for(var x = 0; x < this.width; ++x) {
              row.push(0);
            }
            this.screen.push(row);
          }

          this.outputType = OutputTypes.X;
          this.pos = {
            x: 0,
            y: 0
          };
        },

        startGame: function() {
          this.reset();
          var runner = Intcode.createRunner(this.program);
          if(this.quarters > 0) {
            runner.memory[0] = this.quarters;
          }
          runner.onoutput = this.handleOutput;
          runner.onhalt = this.handleHalt;
          runner.run();
        },

        handleOutput: function(value) {
          switch(this.outputType) {
            case OutputTypes.X:
              this.pos.x = value;
              break;
            case OutputTypes.Y:
              this.pos.y = value;
              break;
            case OutputTypes.TILE:
              if(this.pos.x === -1 && this.pos.y === 0) {
                console.log("Score: " + value);
              } else {
                this.screen[this.pos.y].splice(this.pos.x, 1, value);
              }
              break;
          }

          this.outputType++;
          if(this.outputType > OutputTypes.TILE) {
            this.outputType = OutputTypes.X;
          }
        },

        handleHalt: function() {
          var numBlocks = 0;
          this.screen.forEach(function(row) {
            row.forEach(function(tile) {
              if(tile === 2) {
                numBlocks++;
              }
            });
          });
          console.log("Number of block tiles: " + numBlocks);

          this.finished = true;
        }
      },

      mounted: function() {
        var vm = this;
        getJSON("input").then(function(program) {
          vm.program = program;
          vm.startGame();
        });
      }
    };

    app = new Vue({
      el: "#aoc-arcade",

      components: {
        "aoc-arcade": Arcade
      }
    });
  }

  return {
    init: init
  };
})();
