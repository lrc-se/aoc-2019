"""
Intcode module
"""

class Intcode():
    """Intcode program runner."""

    _opcodes = {
        "1": "add",
        "2": "multiply"
    }

    program = []

    def __init__(self, program=None):
        """Create a new program runner with an optional program."""
        if program is not None:
            self.program = list(program)

    def run(self):
        """Run the program."""
        pointer = 0
        while pointer < len(self.program):
            opcode = str(self.program[pointer])
            positions = self.program[pointer + 1:pointer + 3]
            if opcode == "99":
                return
            elif opcode in self._opcodes:
                result = getattr(self, self._opcodes[opcode])(positions)
                self.program[self.program[pointer + 3]] = result
                pointer += 4
            else:
                raise RuntimeError("Unsupported opcode {0} at position {1}".format(opcode, pointer))

    def add(self, positions):
        """Add two positions together."""
        return self.program[positions[0]] + self.program[positions[1]]

    def multiply(self, positions):
        """Multiply two positions together."""
        return self.program[positions[0]] * self.program[positions[1]]
