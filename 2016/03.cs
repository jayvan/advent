using System;
using System.Collections.Generic;
using System.Linq;

public class Advent03 {
  public static void Main () {
    string line;
    int triangleCount = 0;

    while ((line = Console.ReadLine()) != null) {
      string[] rawSides = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      int[] sides = rawSides.Select(int.Parse).OrderBy(side => side).ToArray();
      if (sides[0] + sides[1] > sides[2]) {
        triangleCount++;
      }
    }

    Console.WriteLine("Number of triangles: " + triangleCount);
  }
}
