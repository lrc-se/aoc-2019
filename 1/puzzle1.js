const fs = require("fs");
const calculateFuel = require("./fuel.js");

fs.readFile("./input.txt", "utf8", (err, data) => {
  if(err) {
    console.error(err.message);
    process.exit(1);
  }

  const fuelData = data.split("\n").filter(line => !!line).map(line => calculateFuel(parseInt(line, 10)));
  const fuelSum = fuelData.reduce((sum, fuel) => sum + fuel);
  console.log(`Total fuel required: ${fuelSum}`);
});
