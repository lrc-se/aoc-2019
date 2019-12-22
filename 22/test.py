"""
Example tests
"""

import unittest
from shuffler import Shuffler


class TestShuffler(unittest.TestCase):
  """Shuffler tests."""

  def test_reset_stack(self):
    shuffler = Shuffler()
    self.assertEqual(shuffler.stack, [])
    shuffler.reset_stack(7)
    self.assertEqual(shuffler.stack, [0, 1, 2, 3, 4, 5, 6])
    shuffler.stack[0] = -1
    shuffler.reset_stack()
    self.assertEqual(shuffler.stack, [0, 1, 2, 3, 4, 5, 6])

  def test_new_stack(self):
    shuffler = Shuffler(10)
    shuffler.deal_into_new_stack()
    self.assertEqual(shuffler.stack, [9, 8, 7, 6, 5, 4, 3, 2, 1, 0])

  def test_cut(self):
    shuffler = Shuffler(10)
    shuffler.cut(3)
    self.assertEqual(shuffler.stack, [3, 4, 5, 6, 7, 8, 9, 0, 1, 2])
    shuffler.reset_stack()
    shuffler.cut(-4)
    self.assertEqual(shuffler.stack, [6, 7, 8, 9, 0, 1, 2, 3, 4, 5])

  def test_increment(self):
    shuffler = Shuffler(10)
    shuffler.deal_with_increment(3)
    self.assertEqual(shuffler.stack, [0, 7, 4, 1, 8, 5, 2, 9, 6, 3])


if __name__ == "__main__":
  unittest.main()
