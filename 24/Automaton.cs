using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AOC2019
{
  public interface IAutomaton
  {
    int Width { get; }
    int Height { get; }
    char[,] Cells { get; }
    string GetTileString();
    void Advance(int steps = 1);
  }

  public class Eris : IAutomaton
  {
    public int Width { get; private set; }
    public int Height { get; private set; }
    public char[,] Cells { get; private set; }

    public Eris(IEnumerable<IEnumerable<char>> cells = null)
    {
      Width = 5;
      Height = 5;
      Cells = new char[Height, Width];
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          Cells[y, x] = (cells != null ? cells.ElementAt(y).ElementAt(x) : '.');
        }
      }
    }

    public string GetTileString()
    {
      var builder = new StringBuilder();
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          builder.Append(Cells[y, x]);
        }
      }
      return builder.ToString();
    }

    public void Advance(int steps = 1)
    {
      for (int i = 0; i < steps; i++)
      {
        // count bugs
        var bugCounts = new int[Height, Width];
        for (int y = 0; y < Height; y++)
        {
          for (int x = 0; x < Width; x++)
          {
            bugCounts[y, x] = CountAdjacentBugs(x, y);
          }
        }

        // update tiles
        for (int y = 0; y < Height; y++)
        {
          for (int x = 0; x < Width; x++)
          {
            if (Cells[y, x] == '#' && bugCounts[y, x] != 1)
            {
              Cells[y, x] = '.';
            }
            else if (Cells[y, x] == '.' && (bugCounts[y, x] == 1 || bugCounts[y, x] == 2))
            {
              Cells[y, x] = '#';
            }
          }
        }
      }
    }

    public int GetBiodiversityRating()
    {
      int rating = 0;
      int exponent = 0;
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          if (Cells[y, x] == '#')
          {
            rating += (int)Math.Pow(2, exponent);
          }
          exponent++;
        }
      }
      return rating;
    }


    private int CountAdjacentBugs(int x, int y)
    {
      int count = 0;

      // top
      if (y > 0 && Cells[y - 1, x] == '#')
      {
        count++;
      }

      // bottom
      if (y < Height - 1 && Cells[y + 1, x] == '#')
      {
        count++;
      }

      // left
      if (x > 0 && Cells[y, x - 1] == '#')
      {
        count++;
      }

      // right
      if (x < Width - 1 && Cells[y, x + 1] == '#')
      {
        count++;
      }

      return count;
    }
  }
}
