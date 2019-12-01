const calculateFuel = require("./fuel.js");

[[12, 2], [14, 2], [1969, 654], [100756, 33583]].forEach(pair => {
  const fuel = calculateFuel(pair[0]);
  console.log(`Mass ${pair[0]} requires fuel ${pair[1]}. Result: ${fuel}`);
});
