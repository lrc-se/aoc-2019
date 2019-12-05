using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace AOC2019
{
  public class Puzzle
  {
    public static void Main()
    {
      Console.WriteLine("TESTS");
      Console.WriteLine("=====");
      RunTests();
      Console.WriteLine();
    }

    public static void RunTests()
    {
      var runner = new IntcodeRunner();
      
      // old tests
      runner.Program = new List<int>() { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 };
      runner.Run();
      Console.WriteLine("Test 1: " + runner.Program.SequenceEqual(new List<int>() { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }));

      runner.Program = new List<int>() { 1, 0, 0, 0, 99 };
      runner.Run();
      Console.WriteLine("Test 2: " + runner.Program.SequenceEqual(new List<int>() { 2, 0, 0, 0, 99 }));

      runner.Program = new List<int>() { 2, 3, 0, 3, 99 };
      runner.Run();
      Console.WriteLine("Test 3: " + runner.Program.SequenceEqual(new List<int>() { 2, 3, 0, 6, 99 }));

      runner.Program = new List<int>() { 2, 4, 4, 5, 99, 0 };
      runner.Run();
      Console.WriteLine("Test 4: " + runner.Program.SequenceEqual(new List<int>() { 2, 4, 4, 5, 99, 9801 }));

      runner.Program = new List<int>() { 1, 1, 1, 4, 99, 5, 6, 0, 99 };
      runner.Run();
      Console.WriteLine("Test 5: " + runner.Program.SequenceEqual(new List<int>() { 30, 1, 1, 4, 2, 5, 6, 0, 99 }));

      // new tests
      runner.Program = new List<int>() { 1002, 4, 3, 4, 33 };
      runner.Run();
      Console.WriteLine("Test 6: " + runner.Program.SequenceEqual(new List<int>() { 1002, 4, 3, 4, 99 }));

      runner.Program = new List<int>() { 1101, 100, -1, 4, 0 };
      runner.Run();
      Console.WriteLine("Test 7: " + runner.Program.SequenceEqual(new List<int>() { 1101, 100, -1, 4, 99 }));
    }
  }
}
