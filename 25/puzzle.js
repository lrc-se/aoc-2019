"use strict";

const fs = require("fs");
const readline = require("readline");
const asciiIntcode = require("./ascii-intcode");


function handleInput() {
  rl.question("> ", command => {
    if(command) {
      this.inputInstruction(command, false);
    } else {
      exit();
    }
  });
}

function exit() {
  rl.close();
}


const input = fs.readFileSync("input.txt", "utf8").split(",").map(code => parseInt(code, 10));
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

const runner = asciiIntcode.createRunner(input, 16384);
runner.on("instructionwaiting", handleInput);
runner.on("halt", exit);
runner.run();
