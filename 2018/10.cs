using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Adcent10 {
  public static void Main() {
    var stars = new List<Vector>();
    var regex = new Regex(@"position=<(.*), (.*)> velocity=<(.*), (.*)>");
    string line;
    int longestWait = int.MinValue;

    while ((line = Console.ReadLine()) != null) {
      var matches = regex.Match(line);
      var star = new Vector(int.Parse(matches.Groups[1].Value),
                            int.Parse(matches.Groups[2].Value),
                            int.Parse(matches.Groups[3].Value),
                            int.Parse(matches.Groups[4].Value));
      longestWait = Math.Max(longestWait, star.Wait);
      stars.Add(star);
    }

    int count = 0;
    while (true) {
      int maxX = int.MinValue;
      int minX = int.MaxValue;
      int maxY = int.MinValue;
      int minY = int.MaxValue;

      foreach (Vector vector in stars) {
        maxX = Math.Max(maxX, vector.X);
        minX = Math.Min(minX, vector.X);
        maxY = Math.Max(maxY, vector.Y);
        minY = Math.Min(minY, vector.Y);
        vector.Update();
      }

      count++;

      if (maxX - minX > 75 || maxY - minY > 25) {
        continue;
      }

      Console.WriteLine(count);
      for (int y = minY; y <= maxY; y++) {
        for (int x = minX; x <= maxX; x++) {
          bool hasStar = false;
          foreach (Vector vector in stars) {
            if (vector.X == x && vector.Y == y) {
              hasStar = true;
              break;
            }
          }

          Console.Write(hasStar ? '#' : '.');
        }

        Console.WriteLine();
      }
    }
  }
}

public class Vector {
  public readonly int xd, yd;

  public int X { get; private set; }
  public int Y { get; private set; }

  public int Wait {
    get {
      return Math.Max(-X / (this.xd == 0 ? 1 : this.xd), -Y / (this.yd == 0 ? 1 : yd));
    }
  }

  public Vector(int x, int y, int xd, int yd) {
    this.X = x;
    this.Y = y;
    this.xd = xd;
    this.yd = yd;
  }

  public void Update(int count = 1) {
    this.X += this.xd * count;
    this.Y += this.yd * count;
  }

  public override string ToString() {
    return $"({X}, {Y}) -> ({this.xd}, {this.yd})";
  }
}
