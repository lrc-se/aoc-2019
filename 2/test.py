import unittest
from intcode import Intcode


class TestIntcode(unittest.TestCase):
    def test_example1(self):
        """Test example 1."""
        runner = Intcode([1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50])
        runner.run()
        self.assertEqual(runner.program, [3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50])

    def test_example2(self):
        """Test example 2."""
        runner = Intcode([1, 0, 0, 0, 99])
        runner.run()
        self.assertEqual(runner.program, [2, 0, 0, 0, 99])

    def test_example3(self):
        """Test example 3."""
        runner = Intcode([2, 3, 0, 3, 99])
        runner.run()
        self.assertEqual(runner.program, [2, 3, 0, 6, 99])

    def test_example4(self):
        """Test example 4."""
        runner = Intcode((2, 4, 4, 5, 99, 0))
        runner.run()
        self.assertEqual(runner.program, [2, 4, 4, 5, 99, 9801])

    def test_example5(self):
        """Test example 5."""
        runner = Intcode((1, 1, 1, 4, 99, 5, 6, 0, 99))
        runner.run()
        self.assertEqual(runner.program, [30, 1, 1, 4, 2, 5, 6, 0, 99])


if __name__ == "__main__":
    unittest.main()
