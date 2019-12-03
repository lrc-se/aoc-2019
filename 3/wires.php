<?php

namespace AOC2019\Wires;


const DELTAS = [
  'U' => [0, 1],
  'R' => [1, 0],
  'D' => [0, -1],
  'L' => [-1, 0],
];


function load_wires_from_file($filename) {
  return array_map(__NAMESPACE__ . '\get_coordinates', preg_split('/\r?\n/', trim(file_get_contents($filename))));
}

function get_coordinates($path) {
  $coords = ['0,0'];
  $x = 0;
  $y = 0;
  foreach(explode(',', $path) as $move) {
    $move = trim($move);
    $direction = strtoupper($move[0]);
    $distance = intval(substr($move, 1));
    if(!array_key_exists($direction, DELTAS)) {
      throw new \Exception("Invalid direction: $direction");
    }
    [$delta_x, $delta_y] = DELTAS[$direction];
    for($i = 0; $i < $distance; $i++) {
      $x += $delta_x;
      $y += $delta_y;
      $coords[] = "$x,$y";
    }
  }
  return $coords;
}

function get_intersections(...$coordinates) {
  $inter = array_intersect(...$coordinates);
  return array_filter($inter, function($element) {
    return ($element != '0,0');
  });
}

function get_closest_distance_from_origin(...$coordinates) {
  $inter = get_intersections(...$coordinates);
  $distances = array_map(function($coord) {
    [$x, $y] = explode(',', $coord);
    return abs(intval($x)) + abs(intval($y));
  }, $inter);
  sort($distances);
  return (!empty($distances) ? $distances[0] : -1);
}

function get_minimum_intersection_steps(...$coordinates) {
  $intersections = get_intersections(...$coordinates);
  $min_steps = -1;
  foreach($intersections as $inter) {
    $sum = 0;
    foreach($coordinates as $coords) {
      $steps = array_search($inter, $coords);
      if($steps !== false) {
        $sum += $steps;
      }
    }
    if($min_steps === -1 || $sum < $min_steps) {
      $min_steps = $sum;
    }
  }
  return $min_steps;
}
