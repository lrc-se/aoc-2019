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
      RunExamples1();
      Console.WriteLine();
      RunPuzzle1();
      Console.WriteLine();
      RunExamples2();
      Console.WriteLine();
      RunPuzzle2();
    }

    private static void RunPuzzle1()
    {
      var scan = LoadState("input.txt");
      var automaton = new Eris(scan);
      Console.WriteLine("PUZZLE 1");
      Console.WriteLine("========");
      FindFirstRepeatedLayout(automaton);
      Console.WriteLine("First repeated layout:");
      PrintScan(automaton);
      Console.WriteLine();
      Console.WriteLine("Biodiversity rating: " + automaton.GetBiodiversityRating());
    }

    private static void RunPuzzle2()
    {
      var scan = LoadState("input.txt");
      var automaton = new RecursiveEris(scan);
      Console.WriteLine("PUZZLE 2");
      Console.WriteLine("========");
      automaton.Advance(200);
      Console.WriteLine("Total number of bugs: " + automaton.CountBugsRecursive());
    }

    private static void RunExamples1()
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

      automaton = new Eris(scan);
      FindFirstRepeatedLayout(automaton);
      Console.WriteLine("First repeated layout:");
      PrintScan(automaton);
      Console.WriteLine();
      Console.WriteLine("Biodiversity rating: " + automaton.GetBiodiversityRating());
    }

    private static void RunExamples2()
    {
      var scan = LoadState("example1.txt");
      var automaton = new RecursiveEris(scan);
      automaton.Advance(10);
      Console.WriteLine("EXAMPLE 2");
      Console.WriteLine("=========");
      Console.WriteLine("Depth 0:");
      PrintScan(automaton);
      Console.WriteLine();

      int counter = 0;
      var parent = automaton.ParentLevel;
      while (parent != null)
      {
        counter--;
        Console.WriteLine("Depth " + counter + ":");
        PrintScan(parent);
        Console.WriteLine();
        parent = parent.ParentLevel;
      }
      counter = 0;
      var child = automaton.ChildLevel;
      while (child != null)
      {
        counter++;
        Console.WriteLine("Depth " + counter + ":");
        PrintScan(child);
        Console.WriteLine();
        child = child.ChildLevel;
      }
      Console.WriteLine("Total number of bugs: " + automaton.CountBugsRecursive());
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

    private static void FindFirstRepeatedLayout(IAutomaton automaton)
    {
      var prevTiles = new HashSet<string>();
      while (true)
      {
        string tiles = automaton.GetTileString();
        if (prevTiles.Contains(tiles))
        {
          return;
        }
        prevTiles.Add(tiles);
        automaton.Advance();
      }
    }
  }
}
