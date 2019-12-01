const fuelLib = require("./fuel.js");

[[12, 2], [14, 2], [1969, 654], [100756, 33583]].forEach(pair => {
  const fuel = fuelLib.calculateFuel(pair[0]);
  console.log(`Mass ${pair[0]} requires fuel ${pair[1]}. Result: ${fuel}`);
});

[[14, 2], [1969, 966], [100756, 50346]].forEach(pair => {
  const fuel = fuelLib.calculateTotalFuel(pair[0]);
  console.log(`Mass ${pair[0]} requires total fuel ${pair[1]}. Result: ${fuel}`);
});
