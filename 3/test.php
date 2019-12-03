<?php

namespace AOC2019\Wires;

require __DIR__ . '/wires.php';

$wires = [
  ['R8,U5,L5,D3', 'U7,R6,D4,L4', 6],
  ['R75,D30,R83,U83,L12,D49,R71,U7,L72', 'U62,R66,U55,R34,D71,R55,D58,R83', 159],
  ['R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51', 'U98,R91,D20,R16,D67,R40,U7,R15,U6,R7', 135],
];

echo "Puzzle 1\n";
echo "========\n\n";
foreach($wires as $i => $wire) {
  $wire1 = get_coordinates($wire[0]);
  $wire2 = get_coordinates($wire[1]);
  $inter = get_intersections($wire1, $wire2);
  $distance = get_closest_distance_from_origin($wire1, $wire2);
  echo 'Example ' . ($i + 1) . "\n";
  echo "---------\n";
  echo 'Intersections: ' . implode(' ', $inter) . "\n";
  echo "Closest distance: $distance\n";
  echo 'Expected distance: ' . $wire[2] . "\n\n";
}


$wires2 = [
  ['R8,U5,L5,D3', 'U7,R6,D4,L4', 30],
  ['R75,D30,R83,U83,L12,D49,R71,U7,L72', 'U62,R66,U55,R34,D71,R55,D58,R83', 610],
  ['R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51', 'U98,R91,D20,R16,D67,R40,U7,R15,U6,R7', 410],
];

echo "\n";
echo "Puzzle 2\n";
echo "========\n\n";foreach($wires2 as $i => $wire) {
  $wire1 = get_coordinates($wire[0]);
  $wire2 = get_coordinates($wire[1]);
  $steps = get_minimum_intersection_steps($wire1, $wire2);
  echo 'Example ' . ($i + 1) . "\n";
  echo "---------\n";
  echo "Minimum steps: $steps\n";
  echo 'Expected steps: ' . $wire[2] . "\n\n";
}
