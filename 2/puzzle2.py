"""
Day 2, puzzle 2
"""

from intcode import Intcode


with open("input.txt", "r") as file:
  program = [int(x) for x in file.read().strip().split(",")]

found = False
runner = Intcode()
for noun in range(0, 100):
  for verb in range(0, 100):
    runner.program = program[:]
    runner.program[1] = noun
    runner.program[2] = verb
    try:
      runner.run()
    except:
      continue

    if runner.program[0] == 19690720:
      found = True
      break

  if found:
    break

if found:
  print("Found noun {0} and verb {1}. Value: {2}".format(noun, verb, 100 * noun + verb))
else:
  print("No matching noun/verb pair found")
