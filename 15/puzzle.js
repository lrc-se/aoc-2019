var Puzzle = (function() {
  "use strict";

  var MovementCommand = {
    NORTH: 1,
    SOUTH: 2,
    WEST: 3,
    EAST: 4
  };
  var TileType = {
    WALL: 0,
    OPEN: 1,
    OXYGEN_SYSTEM: 2,
    OXYGEN: 3,
    UNKNOWN: 10,
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
          oxygenPos: null,
          direction: null,
          steps: [],
          showExport: false,
          oxygenProgress: 0
        };
      },

      computed: {
        numSteps: function() {
          return Math.max(this.steps.length - 1, 0);
        },

        mapExport: function() {
          return JSON.stringify(this.display);
        }
      },

      methods: {
        reset: function() {
          this.display = [];
          for(var y = 0; y < this.height; ++y) {
            var row = [];
            for(var x = 0; x < this.width; ++x) {
              row.push(TileType.UNKNOWN);
            }
            this.display.push(row);
          }
          this.pos = {
            x: Math.floor(this.width / 2),
            y: Math.floor(this.height / 2)
          };
          this.display[this.pos.y][this.pos.x] = TileType.OPEN;
          this.oxygenPos = null;
          this.steps = [this.pos.x + "," + this.pos.y];
          this.oxygenProgress = 0;
        },

        start: function() {
          this.reset();
          computer = Intcode.createRunner(this.program);
          computer.onoutput = this.handleOutput;
          computer.run();
        },

        handleKey: function(e) {
          var vm = this;
          var key = e.key || e.keyCode || e.which;
          switch(key) {
            case "ArrowUp":
            case "Up":
            case 38:
              vm.move(MovementCommand.NORTH);
              break;

            case "ArrowDown":
            case "Down":
            case 40:
              vm.move(MovementCommand.SOUTH);
              break;

            case "ArrowLeft":
            case "Left":
            case 37:
              vm.move(MovementCommand.WEST);
              break;

            case "ArrowRight":
            case "Right":
            case 39:
              vm.move(MovementCommand.EAST);
              break;

            case "m":
            case "M":
              vm.showExport = !vm.showExport;
              break;

            case "l":
            case "L":
              var name = prompt("Map file (.json):");
              if(name) {
                vm.oxygenPos = null;
                getJSON(name).then(function(map) {
                  for(var y = 0; y < map.length; ++y) {
                    vm.$set(vm.display, y, map[y]);
                    if(!vm.oxygenPos) {
                      var oxygenX = map[y].indexOf(TileType.OXYGEN_SYSTEM);
                      if(~oxygenX) {
                        vm.oxygenPos = {
                          x: oxygenX,
                          y: y
                        };
                      }
                    }
                  }
                }).catch(function() {
                  alert("Unable to load map file '" + name + ".json'");
                });
              }
              break;

            case "Space":
            case " ":
            case 32:
              if(!vm.oxygenPos) {
                alert("Oxygen system not found!");
              } else if(vm.oxygenProgress) {
                alert("Oxygen fill already started!");
              }
              vm.fillOxygen();
              break;

            default:
              return;
          }

          e.preventDefault();
        },

        move: function(direction) {
          this.direction = direction;
          computer.enqueueInput(direction);
        },

        handleOutput: function(value) {
          var newPos = {
            x: this.pos.x + deltas[this.direction].x,
            y: this.pos.y + deltas[this.direction].y
          };
          if(this.display[newPos.y][newPos.x] === TileType.UNKNOWN) {
            this.$set(this.display[newPos.y], newPos.x, value);
          }

          if(value === TileType.WALL) {
            return;
          } else if(value === TileType.OXYGEN_SYSTEM) {
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
        },

        fillOxygen: function() {
          var vm = this;
          var oxygenPositions = [];
          for(var y = 0; y < vm.height; ++y) {
            for(var x = 0; x < vm.width; ++x) {
              if(vm.display[y][x] === TileType.OPEN && vm.hasAdjacentOxygen(x, y)) {
                oxygenPositions.push({
                  x: x,
                  y: y
                });
              }
            }
          }
          if(oxygenPositions.length) {
            vm.oxygenProgress++;
            oxygenPositions.forEach(function(position) {
              vm.$set(vm.display[position.y], position.x, TileType.OXYGEN);
            });
            setTimeout(vm.fillOxygen, 30);
          }
        },

        hasAdjacentOxygen: function(x, y) {
          return (
            (y > 0 && (this.display[y - 1][x] === TileType.OXYGEN_SYSTEM || this.display[y - 1][x] === TileType.OXYGEN))
            || (y < this.height - 1 && (this.display[y + 1][x] === TileType.OXYGEN_SYSTEM || this.display[y + 1][x] === TileType.OXYGEN))
            || (x > 0 && (this.display[y][x - 1] === TileType.OXYGEN_SYSTEM || this.display[y][x - 1] === TileType.OXYGEN))
            || (x < this.width - 1 && (this.display[y][x + 1] === TileType.OXYGEN_SYSTEM || this.display[y][x + 1] === TileType.OXYGEN))
          );
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
