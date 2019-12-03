<?php

namespace AOC2019\Wires;

require __DIR__ . '/wires.php';

$wires = load_wires_from_file(__DIR__ . '/input.txt');
echo 'Minimum number of steps: ' . get_minimum_intersection_Steps(...$wires) . "\n";
