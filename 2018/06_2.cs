using System;
using System.Collections.Generic;

namespace advent2018 {
  public class Advent06_2 {
    public static void Main() {
      int regionSize = 0;
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


      for (int y = 0; y < highestY; y++) {
        for (int x = 0; x < highestX; x++) {
          int totalDistance = 0;
          for (int i = 0; i < points.Count; i++) {
            totalDistance += Math.Abs(points[i].Item1 - x) + Math.Abs(points[i].Item2 - y);
          }

          if (totalDistance < 10000) {
            regionSize++;
          }
        }
      }

      Console.WriteLine(regionSize);
    }
  }
}
