using System;
using System.Collections.Generic;
using System.Linq;

public class Advent032 {
  public static void Main () {
    string line = Console.ReadLine();
    int triangleCount = 0;
    int[][] triangles = new int[3][];

    for (int i = 0; i < 3; i++) {
      triangles[i] = new int[3];
    }

    while (line != null) {
      for (int i = 0; i < 3; i++) {
        string[] rawSides = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        for (int j = 0; j < 3; j++) {
          triangles[j][i] = int.Parse(rawSides[j]);
        }

        line = Console.ReadLine();
      }

      for (int i = 0; i < 3; i++) {
        Array.Sort(triangles[i]);
        if (triangles[i][0] + triangles[i][1] > triangles[i][2]) {
          triangleCount++;
        }
      }
    }

    Console.WriteLine("Number of triangles: " + triangleCount);
  }
}
