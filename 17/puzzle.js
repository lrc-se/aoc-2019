"use strict";

const fs = require("fs");
const asciiIntcode = require("./ascii-intcode");


function puzzle1() {
  console.log("PUZZLE 1");
  console.log("========");
  const runner = asciiIntcode.createRunner(input);

  let image = [];
  let row = [];
  runner.on("output", value => {
    if(value != 10) {
      row.push(String.fromCharCode(value));
    } else {
      image.push(row);
      row = [];
    }
  });
  runner.run();

  let height = image.length;
  let width = (height ? image[0].length : 0);
  let alignmentSum = 0;
  for(let y = 0; y < height; ++y) {
    for(let x = 0; x < width; ++x) {
      let char = image[y][x];
      let isIntersection;
      if(char == "#" && x > 0 && x < width - 1 && y > 0 && y < height - 1) {
        let deltas = [[-1, 0], [0, 1], [1, 0], [0, -1]];
        isIntersection = true;
        for(let delta of deltas) {
          if(image[y + delta[0]][x + delta[1]] != "#") {
            isIntersection = false;
            break;
          }
        }
      } else {
        isIntersection = false;
      }

      if(isIntersection) {
        alignmentSum += x * y;
        image[y][x] = "O";
      }
    }
  }

  console.log("Intersections:");
  for(let row of image) {
    console.log(row.join(""));
  }
  console.log("Sum of alignment parameters: " + alignmentSum);
}


const input = fs.readFileSync("input.txt", "utf8").split(",").map(line => parseInt(line, 10));
puzzle1();
