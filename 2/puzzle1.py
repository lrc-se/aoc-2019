"""
Day 2, puzzle 1
"""

from intcode import Intcode


with open("input.txt", "r") as file:
  program = [int(x) for x in file.read().strip().split(",")]

program[1] = 12
program[2] = 2
runner = Intcode(program)
runner.run()

print(runner.program[0])
