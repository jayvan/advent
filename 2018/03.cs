using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace advent2018 {
  public class Advent03 {
    public static void Main() {
      string line;
      var fabric = new Fabric();
      var regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");
      var claims = new List<Tuple<int, int, int, int>>();

      while ((line = Console.ReadLine()) != null) {
        Match match = regex.Match(line);

        int left = int.Parse(match.Groups[2].Value);
        int top = int.Parse(match.Groups[3].Value);
        int width = int.Parse(match.Groups[4].Value);
        int height = int.Parse(match.Groups[5].Value);
        claims.Add(new Tuple<int, int, int, int>(left, top, width, height));
        fabric.Mark(left, top, width, height);
      }

      Console.WriteLine("Overlaps: " + fabric.Overlaps);

      for (int i = 0; i < claims.Count; i++) {
        Tuple<int, int, int, int> claim = claims[i];
        if (fabric.NoOverlaps(claim.Item1, claim.Item2, claim.Item3, claim.Item4)) {
          Console.WriteLine("No overlaps: " + (i + 1));
          Environment.Exit(0);
        }
      }
    }
  }

  public class Fabric {
    private Dictionary<Tuple<int, int>, bool> seen = new Dictionary<Tuple<int, int>, bool>();
    public int Overlaps { get; private set; }

    public void Mark(int left, int top, int width, int height) {
      for (int x = left; x < left + width; x++) {
        for (int y = top; y < top + height; y++) {
          var point = new Tuple<int, int>(x, y);

          if (!seen.ContainsKey(point)) {
            seen.Add(point, false);
          } else if (seen[point] == false) {
            seen[point] = true;
            this.Overlaps = this.Overlaps + 1;
          }
        }
      }
    }

    public bool NoOverlaps(int left, int top, int width, int height) {
      for (int x = left; x < left + width; x++) {
        for (int y = top; y < top + height; y++) {
          var point = new Tuple<int, int>(x, y);

          if (this.seen[point]) {
            return false;
          }
        }
      }

      return true;
    }
  }
}
