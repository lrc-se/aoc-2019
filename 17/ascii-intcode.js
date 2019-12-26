"use strict";

const intcode = require("./intcode");


function handleInput() {
  if(!this._isReceivingInstruction) {
    this.emit("instructionwaiting");
  }
}

function handleOutput(value) {
  if(value <= 255) {
    if(value != 10) {
      this.outputBuffer.push(String.fromCharCode(value));
    } else {
      console.log(this.outputBuffer.join(""));
      this.outputBuffer = [];
    }
  }
}

function inputInstruction(instruction, echo = true) {
  if(echo) {
    console.log(instruction);
  }

  this._isReceivingInstruction = true;
  for(let i = 0; i < instruction.length; ++i) {
    this.enqueueInput(instruction.charCodeAt(i));
  }
  this._isReceivingInstruction = false;
  this.enqueueInput(10);
}


module.exports = {
  createRunner(program, memorySize) {
    const runner = intcode.createRunner(program, memorySize);
    runner.__proto__.inputInstruction = inputInstruction;
    runner._isReceivingInstruction = false;
    runner.outputBuffer = [];
    runner.on("inputwaiting", handleInput);
    runner.on("output", handleOutput);
    return runner;
  }
};
