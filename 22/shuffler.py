"""
Shuffler module
"""

import re


class Shuffler:
  """Card deck shuffler."""

  def __init__(self, card_count=0):
    """Create new instance."""
    self.stack = []
    self.reset_stack(card_count)

  def reset_stack(self, card_count=0):
    """Reset stack to factory order."""
    if card_count < 1:
      card_count = len(self.stack)

    self.stack = [value for value in range(card_count)]

  def deal_into_new_stack(self):
    """Reverse stack."""
    self.stack.reverse()
  
  def cut(self, card_count):
    """Cut stack."""
    tmp = self.stack[:card_count]
    self.stack = self.stack[card_count:]
    self.stack.extend(tmp)

  def deal_with_increment(self, inc):
    """Deal with increment."""
    offset = 0
    size = len(self.stack)
    tmp = self.stack[:]
    for i, value in enumerate(self.stack):
      tmp[offset] = value
      offset += inc
      if offset >= size:
        offset -= size

    self.stack = tmp

  def shuffle(self, actions):
    """Shuffle stack using a sequence of actions."""
    pattern = re.compile(r"-?\d+$")
    for action in actions:
      match = pattern.search(action)
      arg = match.group(0) if match else ""
      method = getattr(self, pattern.sub("", action).strip().replace(" ", "_"))
      if arg:
        method(int(arg))
      else:
        method()
