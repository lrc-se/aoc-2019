<?php

require __DIR__ . '/fft.php';

$fft = new FFT([0, 1, 0, -1]);
echo "EXAMPLES\n";
echo "========\n";
echo 'Example 1: ' . implode('', $fft->process(str_split('12345678'), 4)) . "\n";
echo 'Example 2: ' . substr(implode('', $fft->process(str_split('80871224585914546619083218645595'), 100)), 0, 8) . "\n";
echo 'Example 3: ' . substr(implode('', $fft->process(str_split('19617804207202209144916044189917'), 100)), 0, 8) . "\n";
echo 'Example 4: ' . substr(implode('', $fft->process(str_split('69317163492948606335995924319873'), 100)), 0, 8) . "\n";

$input = trim(file_get_contents(__DIR__ . '/input.txt'));
echo "\n";
echo "PUZZLE 1\n";
echo "========\n";
echo 'First eight digits after 100 phases: ' . substr(implode('', $fft->process(str_split($input), 100)), 0, 8) . "\n";
