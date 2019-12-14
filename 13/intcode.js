var Intcode = (function() {
  "use strict";

  var OPERATIONS = [
    { code: 1, parameterCount: 3 },
    { code: 2, parameterCount: 3 },
    { code: 3, parameterCount: 1 },
    { code: 4, parameterCount: 1 },
    { code: 5, parameterCount: 2 },
    { code: 6, parameterCount: 2 },
    { code: 7, parameterCount: 3 },
    { code: 8, parameterCount: 3 },
    { code: 9, parameterCount: 1 },
    { code: 99, parameterCount: 0 }
  ];
  var DEFAULT_MEMORY_SIZE = 1024;

  var ParameterMode = {
    POSITION: 0,
    IMMEDIATE: 1,
    RELATIVE: 2
  };

  var IntcodeRunner = (function() {
    function run() {
      this._pointer = 0;
      this._relativeBase = 0;
      runProgram.call(this);
    }

    function enqueueInput(value) {
      this._inputQueue.push(value);
      if(this._waitingForInput) {
        this._waitingForInput = false;
        runProgram.call(this);
      }
    }


    function runProgram() {
      var getValue = getParameterValue.bind(this);
      var write = writeToMemory.bind(this);
      var fire = fireEvent.bind(this);
      while(this._pointer < this.memory.length) {
        var instruction = parseInstruction(this.memory[this._pointer]);
        var operation = OPERATIONS.filter(function(op) {
          return (op.code === instruction.opcode);
        })[0];
        if(!operation) {
          throw Error("Unsupported opcode " + instruction.opcode + " at position " + this._pointer);
        }

        var parameters = this.memory.slice(this._pointer + 1, this._pointer + operation.parameterCount + 1).map(function(p, i) {
          var mode = instruction.parameterModes[i];
          return {
            value: p,
            mode: (typeof mode != "undefined" ? mode : ParameterMode.POSITION)
          };
        });

        switch(operation.code) {
          case 1:
            write(parameters[2], getValue(parameters[0]) + getValue(parameters[1]));
            break;

          case 2:
            write(parameters[2], getValue(parameters[0]) * getValue(parameters[1]));
            break;

          case 3:
            if(this._inputQueue.length) {
              write(parameters[0], this._inputQueue.shift());
            } else {
              this._waitingForInput = true;
              fire("inputwaiting");
              return;
            }
            break;

          case 4:
            fire("output", getValue(parameters[0]));
            break;

          case 5:
            if(getValue(parameters[0]) !== 0) {
              this._pointer = getValue(parameters[1]);
              continue;
            }
            break;
            
          case 6:
            if(getValue(parameters[0]) === 0) {
              this._pointer = getValue(parameters[1]);
              continue;
            }
            break;

          case 7:
            write(parameters[2], (getValue(parameters[0]) < getValue(parameters[1]) ? 1 : 0));
            break;

          case 8:
            write(parameters[2], (getValue(parameters[0]) === getValue(parameters[1]) ? 1 : 0));
            break;

          case 9:
            this._relativeBase += getValue(parameters[0]);
            break;

          case 99:
            fire("halt");
            return;
        }

        this._pointer += operation.parameterCount + 1;
      }

      fire("halt");
    }

    function getParameterValue(parameter) {
      var value;
      switch(parameter.mode) {
        case ParameterMode.IMMEDIATE:
          value = parameter.value;
          break;
        case ParameterMode.RELATIVE:
          value = this.memory[this._relativeBase + parameter.value];
          break;
        case ParameterMode.POSITION:
        default:
          value = this.memory[parameter.value];
      }
      return value;
    }

    function writeToMemory(positionParameter, value) {
      if(positionParameter.mode === ParameterMode.IMMEDIATE) {
        throw Error("Write parameter for instruction at position " + this._pointer + " cannot be immediate");
      }

      var position = positionParameter.value;
      if(positionParameter.mode === ParameterMode.RELATIVE) {
        position += this._relativeBase;
      }
      this.memory[position] = value;
    }

    function fireEvent(name, data) {
      var listener = this["on" + name];
      if(typeof listener == "function") {
        if(typeof data == "undefined") {
          listener();
        } else {
          listener(data);
        }
      }
    }

    return {
      run: run,
      enqueueInput: enqueueInput
    };
  })();


  function parseInstruction(code) {
    var instruction = {
      parameterModes: []
    };
    var modes = Math.floor(code / 100);
    instruction.opcode = code - modes * 100;
    while (modes >= 1) {
      var next = Math.floor(modes / 10);
      instruction.parameterModes.push(modes - next * 10);
      modes = next;
    }
    return instruction;
  }

  function createRunner(program, memorySize) {
    var runner = Object.create(IntcodeRunner);
    runner._pointer = 0;
    runner._relativeBase = 0;
    runner._inputQueue = [];
    runner._waitingForInput = false;
    runner.oninputwaiting = null;
    runner.onoutput = null;
    runner.onhalt = null;

    var programSize;
    if(Array.isArray(program)) {
      programSize = program.length;
      runner.memory = program.slice();
    } else {
      programSize = 0;
      runner.memory = [];
    }
    for(var i = 0; i < (memorySize || DEFAULT_MEMORY_SIZE) - programSize; ++i) {
      runner.memory.push(0);
    }

    return runner;
  }

  return {
    createRunner: createRunner
  };
})();
