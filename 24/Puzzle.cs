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
      RunExamples();
    }

    private static void RunExamples()
    {
      var scan = LoadState("example1.txt");
      var automaton = new Eris(scan);
      Console.WriteLine("EXAMPLE 1");
      Console.WriteLine("=========");
      Console.WriteLine();
      Console.WriteLine("Initial state:");
      PrintScan(automaton);
      Console.WriteLine();
      for (int i = 1; i < 5; i++)
      {
        automaton.Advance();
        Console.WriteLine("After " + i + " minute(s):");
        PrintScan(automaton);
        Console.WriteLine();
      }
    }

    private static IList<string> LoadState(string filename)
    {
      return System.IO.File.ReadAllLines(filename);
    }

    private static void PrintScan(IAutomaton automaton)
    {
      for (int y = 0; y < automaton.Height; y++)
      {
        for (int x = 0; x < automaton.Width; x++)
        {
          Console.Write(automaton.Cells[y, x]);
        }
        Console.WriteLine();
      }
    }
  }
}
