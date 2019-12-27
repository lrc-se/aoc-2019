var Puzzle = (function() {
  "use strict";

  var MovementCommand = {
    NORTH: 1,
    SOUTH: 2,
    WEST: 3,
    EAST: 4
  };
  var StatusCode = {
    WALL: 0,
    MOVED: 1,
    FOUND: 2
  };
  var deltas = {
    1: { x: 0, y: -1 },
    2: { x: 0, y: 1 },
    3: { x: -1, y: 0 },
    4: { x: 1, y: 0 }
  };

  var app;
  var computer;

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
    var Area = {
      props: {
        width: {
          type: Number,
          default: 50
        },

        height: {
          type: Number,
          default: 50
        }
      },

      data: function() {
        return {
          program: null,
          display: [],
          pos: {},
          oxygenPos: {},
          direction: null,
          steps: []
        };
      },

      methods: {
        reset: function() {
          this.display = [];
          for(var y = 0; y < this.height; ++y) {
            var row = [];
            for(var x = 0; x < this.width; ++x) {
              row.push(100);
            }
            this.display.push(row);
          }
          this.pos = {
            x: Math.floor(this.width / 2),
            y: Math.floor(this.height / 2)
          };
          this.display[this.pos.y][this.pos.x] = StatusCode.MOVED;
          this.steps = [this.pos.x + "," + this.pos.y];
        },

        start: function() {
          this.reset();
          computer = Intcode.createRunner(this.program);
          computer.onoutput = this.handleOutput;
          computer.run();
        },

        handleKey: function(e) {
          var key = e.key || e.keyCode || e.which;
          switch(key) {
            case "ArrowUp":
            case "Up":
            case 38:
              this.direction = MovementCommand.NORTH;
              break;
            case "ArrowDown":
            case "Down":
            case 40:
              this.direction = MovementCommand.SOUTH;
              break;
            case "ArrowLeft":
            case "Left":
            case 37:
              this.direction = MovementCommand.WEST;
              break;
            case "ArrowRight":
            case "Right":
            case 39:
              this.direction = MovementCommand.EAST;
              break;
            default:
              return;
          }
          e.preventDefault();
          computer.enqueueInput(this.direction);
        },

        handleOutput: function(value) {
          var newPos = {
            x: this.pos.x + deltas[this.direction].x,
            y: this.pos.y + deltas[this.direction].y
          };
          this.$set(this.display[newPos.y], newPos.x, value);

          if(value === StatusCode.WALL) {
            return;
          } else if(value === StatusCode.FOUND) {
            this.oxygenPos = {
              x: newPos.x,
              y: newPos.y
            };
          }

          this.pos = newPos;
          var coords = newPos.x + "," + newPos.y;
          if(!~this.steps.indexOf(coords)) {
            this.steps.push(coords);
          } else {
            this.steps.pop();
          }
        },

        isDroidPos: function(x, y) {
          return (this.pos.x == x && this.pos.y == y);
        }
      },

      mounted: function() {
        var vm = this;
        getJSON("input").then(function(program) {
          vm.program = program;
          document.addEventListener("keydown", vm.handleKey);
          vm.start();
        });
      }
    };

    app = new Vue({
      el: "#aoc-droid-app",

      components: {
        "aoc-area": Area
      }
    });
  }

  return {
    init: init
  };
})();
