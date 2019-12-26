"use strict";

const OPERATIONS = {
  1: { parameterCount: 3 },
  2: { parameterCount: 3 },
  3: { parameterCount: 1 },
  4: { parameterCount: 1 },
  5: { parameterCount: 2 },
  6: { parameterCount: 2 },
  7: { parameterCount: 3 },
  8: { parameterCount: 3 },
  9: { parameterCount: 1 },
  99: { parameterCount: 0 }
};
const DEFAULT_MEMORY_SIZE = 4096;

const ParameterMode = {
  POSITION: 0,
  IMMEDIATE: 1,
  RELATIVE: 2
};


function parseInstruction(code) {
  const codeStr = code.toString();
  const instruction = {
    opcode: +codeStr.slice(-2),
    parameterModes: []
  };
  for(let i = codeStr.length - 3; i >= 0; --i) {
    instruction.parameterModes.push(+codeStr[i]);
  }
  return instruction;
}

function runProgram() {
  const getValue = getParameterValue.bind(this);
  const write = writeToMemory.bind(this);
  while(this._pointer < this.memory.length) {
    let instruction = parseInstruction(this.memory[this._pointer]);
    if(!(instruction.opcode in OPERATIONS)) {
      throw Error(`Unsupported opcode ${instruction.opcode} at position ${this._pointer}`);
    }

    let parameters = this.memory.slice(this._pointer + 1, this._pointer + OPERATIONS[instruction.opcode].parameterCount + 1).map((p, i) => ({
      value: p,
      mode: (typeof instruction.parameterModes[i] != "undefined" ? instruction.parameterModes[i] : ParameterMode.POSITION)
    }));

    switch(instruction.opcode) {
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
          this.emit("inputwaiting");
          return;
        }
        break;

      case 4:
        this.emit("output", getValue(parameters[0]));
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
        write(parameters[2], +(getValue(parameters[0]) < getValue(parameters[1])));
        break;

      case 8:
        write(parameters[2], +(getValue(parameters[0]) === getValue(parameters[1])));
        break;

      case 9:
        this._relativeBase += getValue(parameters[0]);
        break;

      case 99:
        this.emit("halt");
        return;
    }

    this._pointer += OPERATIONS[instruction.opcode].parameterCount + 1;
  }

  this.emit("halt");
}

function getParameterValue(parameter) {
  let value;
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
    throw Error(`Write parameter for instruction at position ${this._pointer} cannot be immediate`);
  }

  let position = positionParameter.value;
  if(positionParameter.mode === ParameterMode.RELATIVE) {
    position += this._relativeBase;
  }
  if(position < 0 || position >= this.memory.length) {
    throw Error(`Memory position ${position} is out of bounds`);
  }

  this.memory[position] = value;
}


const IntcodeRunner = {
  run() {
    this._pointer = 0;
    this._relativeBase = 0;
    runProgram.call(this);
  },

  enqueueInput(value) {
    this._inputQueue.push(value);
    if(this._waitingForInput) {
      this._waitingForInput = false;
      runProgram.call(this);
    }
  }
};

Object.setPrototypeOf(IntcodeRunner, require("events").EventEmitter.prototype);


module.exports = {
  createRunner(program, memorySize) {
    const runner = Object.create(IntcodeRunner);
    runner._pointer = 0;
    runner._relativeBase = 0;
    runner._inputQueue = [];
    runner._waitingForInput = false;

    let programSize;
    if(Array.isArray(program)) {
      programSize = program.length;
      runner.memory = program.slice();
    } else {
      programSize = 0;
      runner.memory = [];
    }
    for(let i = 0; i < (memorySize || DEFAULT_MEMORY_SIZE) - programSize; ++i) {
      runner.memory.push(0);
    }

    return runner;
  }
};
