using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AOC2019
{
  public class RecursiveEris : IAutomaton
  {
    private const int Center = 2;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public char[,] Cells { get; private set; }
    public RecursiveEris ParentLevel { get; private set; }
    public RecursiveEris ChildLevel { get; private set; }


    public RecursiveEris(IEnumerable<IEnumerable<char>> cells = null, RecursiveEris parentLevel = null, RecursiveEris childLevel = null)
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
      Cells[Center, Center] = '?';
      ParentLevel = parentLevel;
      ChildLevel = childLevel;
    }

    public string GetTileString()
    {
      return GetTileStringFor(Cells);
    }

    public void Advance(int steps)
    {
      Advance(steps, true, true);
    }

    public void Advance(int steps = 1, bool advanceParent = true, bool advanceChild = true)
    {
      for (int i = 0; i < steps; i++)
      {
        // save next and current layouts
        var nextLayout = GetNextLayout();
        string tiles = GetTileString();
        string nextTiles = GetTileStringFor(nextLayout);

        // handle recursive levels
        if (advanceParent)
        {
          // add parent level if layout will change
          if (nextTiles != tiles && ParentLevel == null)
          {
            ParentLevel = new RecursiveEris(childLevel: this);
          }

          // advance parent level, if any
          if (ParentLevel != null)
          {
            ParentLevel.Advance(advanceChild: false);
          }
        }
        if (advanceChild)
        {
          // add child level if layout will change
          if (nextTiles != tiles && ChildLevel == null)
          {
            ChildLevel = new RecursiveEris(parentLevel: this);
          }

          // advance child level, if any
          if (ChildLevel != null)
          {
            ChildLevel.Advance(advanceParent: false);
          }
        }

        // update tiles
        Cells = nextLayout;
      }
    }

    public int CountBugs()
    {
      int count = 0;
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          if (Cells[y, x] == '#')
          {
            count++;
          }
        }
      }
      return count;
    }

    public int CountBugsRecursive()
    {
      int count = CountBugs();

      // count parent levels, if any
      var parent = ParentLevel;
      while (parent != null)
      {
        count += parent.CountBugs();
        parent = parent.ParentLevel;
      }

      // count child levels, if any
      var child = ChildLevel;
      while (child != null)
      {
        count += child.CountBugs();
        child = child.ChildLevel;
      }

      return count;
    }


    private string GetTileStringFor(char[,] cells)
    {
      var builder = new StringBuilder();
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          builder.Append(cells[y, x]);
        }
      }
      return builder.ToString();
    }

    private int CountAdjacentBugs(int x, int y)
    {
      int count = 0;

      // top
      if (y > 0) {
        if (x == Center && y == Center + 1)
        {
          // check bordering child level, if any
          if (ChildLevel != null)
          {
            for (int childX = 0; childX < ChildLevel.Width; childX++)
            {
              if (ChildLevel.Cells[ChildLevel.Height - 1, childX] == '#')
              {
                count++;
              }
            }
          }
        }
        else if (Cells[y - 1, x] == '#')
        {
          count++;
        }
      }
      // check bordering parent level, if any
      else if (ParentLevel != null && ParentLevel.Cells[Center - 1, Center] == '#')
      {
        count++;
      }

      // bottom
      if (y < Height - 1) {
        if (x == Center && y == Center - 1)
        {
          // check bordering child level, if any
          if (ChildLevel != null)
          {
            for (int childX = 0; childX < ChildLevel.Width; childX++)
            {
              if (ChildLevel.Cells[0, childX] == '#')
              {
                count++;
              }
            }
          }
        }
        else if (Cells[y + 1, x] == '#')
        {
          count++;
        }
      }
      // check bordering parent level, if any
      else if (ParentLevel != null && ParentLevel.Cells[Center + 1, Center] == '#')
      {
        count++;
      }

      // left
      if (x > 0) {
        if (x == Center + 1 && y == Center)
        {
          // check bordering child level, if any
          if (ChildLevel != null)
          {
            for (int childY = 0; childY < ChildLevel.Height; childY++)
            {
              if (ChildLevel.Cells[childY, ChildLevel.Width - 1] == '#')
              {
                count++;
              }
            }
          }
        }
        else if (Cells[y, x - 1] == '#')
        {
          count++;
        }
      }
      // check bordering parent level, if any
      else if (ParentLevel != null && ParentLevel.Cells[Center, Center - 1] == '#')
      {
        count++;
      }

      // right
      if (x < Width - 1) {
        if (x == Center - 1 && y == Center)
        {
          // check bordering child level, if any
          if (ChildLevel != null)
          {
            for (int childY = 0; childY < ChildLevel.Height; childY++)
            {
              if (ChildLevel.Cells[childY, 0] == '#')
              {
                count++;
              }
            }
          }
        }
        else if (Cells[y, x + 1] == '#')
        {
          count++;
        }
      }
      // check bordering parent level, if any
      else if (ParentLevel != null && ParentLevel.Cells[Center, Center + 1] == '#')
      {
        count++;
      }

      return count;
    }

    private char[,] GetNextLayout()
    {
      // count bugs
      var bugCounts = new int[Height, Width];
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          if (Cells[y, x] != '?')
          {
            bugCounts[y, x] = CountAdjacentBugs(x, y);
          }
        }
      }

      // produce new cell layout
      var cells = new char[Height, Width];
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          if (Cells[y, x] == '.' && (bugCounts[y, x] == 1 || bugCounts[y, x] == 2))
          {
            cells[y, x] = '#';
          }
          else if (Cells[y, x] == '#' && bugCounts[y, x] != 1)
          {
            cells[y, x] = '.';
          }
          else
          {
            cells[y, x] = Cells[y, x];
          }
        }
      }

      return cells;
    }
  }
}
