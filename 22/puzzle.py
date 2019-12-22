"""
AOC 2019: Day 22
"""

from shuffler import Shuffler


CARD_COUNT = 10007

def load_actions(filename="input.txt"):
  """Load actions from file."""
  with open(filename) as f:
    return f.readlines()

def puzzle1(actions):
  """Run puzzle 1."""
  shuffler = Shuffler(CARD_COUNT)
  shuffler.shuffle(actions)

  print("PUZZLE 1")
  print("========")
  print("Position of card 2019: {0}".format(shuffler.stack.index(2019)))


if __name__ == "__main__":
  actions = load_actions()
  puzzle1(actions)