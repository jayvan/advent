using System;
using System.Collections.Generic;

public class Advent03 {
  static List<string> lines = new List<string>();

  public static void Main() {
    string line;

    while ((line = Console.ReadLine()) != null) {
      lines.Add(line);
    }

    long s1 = TreesHitForSlope(1, 1);
    long s2 = TreesHitForSlope(3, 1);
    long s3 = TreesHitForSlope(5, 1);
    long s4 = TreesHitForSlope(7, 1);
    long s5 = TreesHitForSlope(1, 2);

    Console.WriteLine(s1);
    Console.WriteLine(s2);
    Console.WriteLine(s3);
    Console.WriteLine(s4);
    Console.WriteLine(s5);
    Console.WriteLine(s1 * s2 * s3 * s4 * s5);
  }

  private static bool IsTree(int x, int y) {
    x %= lines[0].Length;
    return lines[y][x] == '#';
  }

  private static int TreesHitForSlope(int dx, int dy) {
    int x = 0;
    int y = 0;
    int treeCount = 0;

    while (y < lines.Count) {
      if (IsTree(x, y)) {
        treeCount++;
      }

      x += dx;
      y += dy;
    }

    return treeCount;
  }
}
