using System;

namespace AOC2019
{
  public static class MathUtils
  {
    public static long Gcd(long a, long b)
    {
      if (b == 0)
      {
        return a;
      }
      return Gcd(b, a % b);
    }

    public static long Lcm(long a, long b)
    {
      return Math.Abs(a * b) / Gcd(a, b);
    }
  }
}