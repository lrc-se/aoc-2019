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
      var simulator = new MotionSimulator();

      Console.WriteLine("EXAMPLE 1");
      Console.WriteLine("=========");
      simulator.Bodies = new List<Body>()
      {
        new Body(new Vector(-1, 0, 2)),
        new Body(new Vector(2, -10, -7)),
        new Body(new Vector(4, -8, 8)),
        new Body(new Vector(3, 5, -1))
      };
      for (int i = 0; i < 10; i++) {
        Console.WriteLine("After " + (i + 1) + " step(s):");
        simulator.Run();
        PrintStatus(simulator);
        Console.WriteLine();
      }
      Console.WriteLine("Total energy: " + simulator.Bodies.Sum(b => b.TotalEnergy));
      Console.WriteLine();

      Console.WriteLine("EXAMPLE 2");
      Console.WriteLine("=========");
      simulator.Bodies = new List<Body>()
      {
        new Body(new Vector(-8, -10, 0)),
        new Body(new Vector(5, 5, 10)),
        new Body(new Vector(2, -7, 3)),
        new Body(new Vector(9, -8, -3))
      };
      for (int i = 0; i < 100; i += 10) {
        Console.WriteLine("After " + i + " step(s):");
        simulator.Run(10);
        PrintStatus(simulator);
        Console.WriteLine();
      }
      Console.WriteLine("Total energy: " + simulator.Bodies.Sum(b => b.TotalEnergy));
    }

    private static void PrintStatus(MotionSimulator simulator)
    {
      foreach (var body in simulator.Bodies)
      {
        Console.Write("pos=<x=" + body.Position.X + ", y=" + body.Position.Y + ", z=" + body.Position.Z + ">, ");
        Console.Write("vel=<x=" + body.Velocity.X + ", y=" + body.Velocity.Y + ", z=" + body.Velocity.Z + ">");
        Console.WriteLine();
      }
    }
  }
}