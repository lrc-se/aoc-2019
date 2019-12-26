"use strict";

const fs = require("fs");
const asciiIntcode = require("./ascii-intcode");


function handleOutput(value) {
  if(value > 255) {
    console.log(`Hull damage: ${value}`);
  }
}

function runDroid(program, instructions) {
  return new Promise((resolve, reject) => {
    const asciiRunner = asciiIntcode.createRunner(program);
    asciiRunner.on("output", handleOutput);
    asciiRunner.on("halt", resolve);
    asciiRunner.run();
    for(let instruction of instructions) {
      asciiRunner.inputInstruction(instruction);
    }
  });
}

function puzzle1() {
  console.log("PUZZLE 1");
  console.log("========");
  return runDroid(input, [
    "OR D J",
    "NOT C T",
    "AND T J",
    "NOT A T",
    "OR T J",
    "WALK"
  ]);
}


const input = fs.readFileSync("input.txt", "utf8").split(",").map(line => parseInt(line, 10));
puzzle1().catch(ex => {
  console.error(`Error: ${ex.message}`);
});
