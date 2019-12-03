<?php

namespace AOC2019\Wires;

require __DIR__ . '/wires.php';

$paths = load_paths_from_file(__DIR__ . '/input.txt');
$wires = array_map(function($path) {
  return get_coordinates($path);
}, $paths);

echo 'Intersections: ' . implode(' ', get_intersections(...$wires)) . "\n";
echo 'Closest distance: ' . get_closest_distance_from_origin(...$wires) . "\n";
