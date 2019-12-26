<?php

class FFT {
  private $pattern;

  public function __construct($pattern) {
    $this->pattern = $pattern;
  }

  public function process($signal, $numPhases = 1) {
    $input = $signal;
    $input_size = count($input);
    $pattern_size = count($this->pattern);
    for($i = 0; $i < $numPhases; $i++) {
      $output = [];
      for($input_index = 1; $input_index <= $input_size; $input_index++) {
        if($input_index == 1) {
          $pattern_index = 1;
          $pattern_counter = 0;
        } else {
          $pattern_index = 0;
          $pattern_counter = 1;
        }

        $output_value = 0;
        foreach($input as $input_value) {
          $output_value += $input_value * $this->pattern[$pattern_index];

          $pattern_counter++;
          if($pattern_counter == $input_index) {
            $pattern_counter = 0;
            $pattern_index++;
          }
          if($pattern_index == $pattern_size) {
            $pattern_index = 0;
          }
        }

        $output[] = abs($output_value % 10);
      }

      $input = $output;
    }

    return $output;
  }
}
