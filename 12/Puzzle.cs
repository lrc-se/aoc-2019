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

      var simulator = new MotionSimulator(LoadBodies());
      simulator.Run(1000);
      Console.WriteLine("PUZZLE 1");
      Console.WriteLine("========");
      Console.WriteLine("Total energy: " + simulator.Bodies.Sum(b => b.TotalEnergy));
      Console.WriteLine();

      RunExamples2();
      Console.WriteLine();

      Console.WriteLine("PUZZLE 2");
      Console.WriteLine("========");
      Console.WriteLine("Total repeat steps: " + FindRepeatSteps(LoadBodies()));
    }


    private static IList<Body> LoadBodies(string filename = "input.txt")
    {
      var bodies = new List<Body>();
      var coordinates = System.IO.File.ReadAllLines(filename);
      var re = new System.Text.RegularExpressions.Regex(@"\<x=(-?\d+),\s*y=(-?\d+),\s*z=(-?\d+)\>");
      foreach (string coordinate in coordinates)
      {
        var matches = re.Matches(coordinate);
        bodies.Add(new Body(new Vector(
          Convert.ToInt32(matches[0].Groups[1].Value),
          Convert.ToInt32(matches[0].Groups[2].Value),
          Convert.ToInt32(matches[0].Groups[3].Value)
        )));
      }
      return bodies;
    }

    private static long FindRepeatSteps(IEnumerable<Body> bodies, long maxSteps = 1000000000)
    {
      var simulator = new MotionSimulator();
      var steps = new long[] { -1, -1, -1 };

      for (int i = 0; i < 3; i++)
      {
        simulator.Bodies = bodies.Select(b => b.Clone()).ToList();
        var axis = (Axis)i;
        string state = simulator.GetAxisState(axis);

        for (long j = 0; j < maxSteps; j++)
        {
          simulator.Run();
          if (simulator.GetAxisState(axis) == state)
          {
            steps[i] = j + 1;
            break;
          }
        }
      }

      if (steps.Any(s => s == -1))
      {
        return -1;
      }
      return MathUtils.Lcm(MathUtils.Lcm(steps[0], steps[1]), steps[2]);
    }

    private static void RunExamples1()
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

    private static void RunExamples2()
    {
      IList<Body> bodies;

      Console.WriteLine("EXAMPLE 3");
      Console.WriteLine("=========");
      bodies = new List<Body>()
      {
        new Body(new Vector(-1, 0, 2)),
        new Body(new Vector(2, -10, -7)),
        new Body(new Vector(4, -8, 8)),
        new Body(new Vector(3, 5, -1))
      };
      Console.WriteLine("Total repeat steps: " + FindRepeatSteps(bodies));
      Console.WriteLine();

      Console.WriteLine("EXAMPLE 4");
      Console.WriteLine("=========");
      bodies = new List<Body>()
      {
        new Body(new Vector(-8, -10, 0)),
        new Body(new Vector(5, 5, 10)),
        new Body(new Vector(2, -7, 3)),
        new Body(new Vector(9, -8, -3))
      };
      Console.WriteLine("Total repeat steps: " + FindRepeatSteps(bodies));
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