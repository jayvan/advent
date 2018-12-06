using System;
using System.Collections.Generic;

namespace advent2018 {
  public class Advent06 {
    public static void Main() {
      int highestX = 0;
      int highestY = 0;
      string line;
      var points = new List<Tuple<int, int>>();

      while ((line = Console.ReadLine()) != null) {
        string[] parts = line.Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries);
        var point = new Tuple<int, int>(int.Parse(parts[0]), int.Parse(parts[1]));
        highestX = Math.Max(highestX, point.Item1);
        highestY = Math.Max(highestY, point.Item2);
        points.Add(point);
      }

      var blacklist = new HashSet<int>();
      var counts = new Dictionary<int, int>();

      for (int y = -25; y < highestY + 25; y++) {
        for (int x = -25; x < highestX + 25; x++) {
          int minDistance = int.MaxValue;
          int minIndex = -1;

          for (int i = 0; i < points.Count; i++) {
            int distance = Math.Abs(points[i].Item1 - x) + Math.Abs(points[i].Item2 - y);

            if (distance < minDistance) {
              minDistance = distance;
              minIndex = i;
            } else if (distance == minDistance) {
              minIndex = -1;
            }
          }

          if (counts.ContainsKey(minIndex)) {
            counts[minIndex]++;
          } else {
            counts.Add(minIndex, 1);
          }

          if (minIndex == -1) {
            Console.Write('.');
          } else if (points.Contains(new Tuple<int, int>(x, y))) {
            Console.Write((char)('A' + minIndex));
          } else {
            Console.Write((char)('a' + minIndex));
          }

          if (x == -25 || y == -25 || x == highestX + 24 || y == highestY + 24) {
            blacklist.Add(minIndex);
          }
        }

        Console.Write("\n");
      }

      int highest = Int32.MinValue;

      foreach (KeyValuePair<int, int> kvp in counts) {
        if (blacklist.Contains(kvp.Key)) {
          continue;
        }

        if (kvp.Value > highest) {
          highest = kvp.Value;
        }
      }

      Console.WriteLine(highest);
    }
  }
}
