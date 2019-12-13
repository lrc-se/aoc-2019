using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace AOC2019
{
  public class MotionSimulator
  {
    public IList<Body> Bodies { get; set; }


    public MotionSimulator(IEnumerable<Body> bodies = null)
    {
      Bodies = (bodies != null ? bodies.ToList() : new List<Body>());
    }

    public void Run(int steps = 1)
    {
      for (int i = 0; i < steps; i++)
      {
        ApplyGravity();
        UpdatePositions();
      }
    }


    private IEnumerable<Tuple<Body, Body>> GetPairs()
    {
      var pairs = new List<Tuple<Body, Body>>();
      for (int i = 0; i < Bodies.Count - 1; i++)
      {
        for (int j = i + 1; j < Bodies.Count; j++)
        {
          pairs.Add(Tuple.Create(Bodies[i], Bodies[j]));
        }
      }
      return pairs;
    }

    private void ApplyGravity()
    {
      int delta;
      foreach (var pair in GetPairs())
      {
        delta = Math.Sign(pair.Item1.Position.X - pair.Item2.Position.X);
        pair.Item1.Velocity.X -= delta;
        pair.Item2.Velocity.X += delta;
        delta = Math.Sign(pair.Item1.Position.Y - pair.Item2.Position.Y);
        pair.Item1.Velocity.Y -= delta;
        pair.Item2.Velocity.Y += delta;
        delta = Math.Sign(pair.Item1.Position.Z - pair.Item2.Position.Z);
        pair.Item1.Velocity.Z -= delta;
        pair.Item2.Velocity.Z += delta;
      }
    }

    private void UpdatePositions()
    {
      foreach (var body in Bodies)
      {
        body.Position.X += body.Velocity.X;
        body.Position.Y += body.Velocity.Y;
        body.Position.Z += body.Velocity.Z;
      }
    }
  }

  public class Body
  {
    public Vector Position { get; set; }
    public Vector Velocity { get; set; }

    public int PotentialEnergy
    {
      get
      {
        return Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);
      }
    }

    public int KineticEnergy
    {
      get
      {
        return Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);
      }
    }

    public int TotalEnergy
    {
      get
      {
        return PotentialEnergy * KineticEnergy;
      }
    }


    public Body(Vector position, Vector velocity = null)
    {
      Position = position;
      Velocity = velocity ?? new Vector(0, 0, 0);
    }
  }

  public class Vector
  {
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Vector(int x, int y, int z)
    {
      X = x;
      Y = y;
      Z = z;
    }
  }
}