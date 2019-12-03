<?php

namespace AOC2019\Wires;

require __DIR__ . '/wires.php';

$wires = load_wires_from_file(__DIR__ . '/input.txt');
echo 'Intersections: ' . implode(' ', get_intersections(...$wires)) . "\n";
echo 'Closest distance: ' . get_closest_distance_from_origin(...$wires) . "\n";
