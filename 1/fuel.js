function calculateFuel(mass) {
  return Math.floor(mass / 3) - 2;
}

function calculateTotalFuel(mass) {
  let total = 0;
  let fuel = mass;
  do {
    fuel = calculateFuel(fuel);
    total += (fuel > 0 ? fuel : 0);
  } while(fuel > 0);
  return total;
}

module.exports = {
  calculateFuel,
  calculateTotalFuel
};
