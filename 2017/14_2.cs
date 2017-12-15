using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class Advent14 {
  private struct Point {
    public readonly int X;
    public readonly int Y;

    public Point(int x, int y) {
      X = x;
      Y = y;
    }
  }

  public static void Main() {
    string input = Console.ReadLine();

    var unvisited = new List<Point>();
    var grid = new bool[128, 128];

    for (int y = 0; y < 128; y++) {
      string row = KnotHash($"{input}-{y}");

      for (int x = 0; x < 128; x++) {
        if (row[x] == '1') {
          grid[x, y] = true;
          unvisited.Add(new Point(x, y));
        }
      }
    }

    int regions = 0;
    while (unvisited.Count > 0) {
      regions++;
      var seen = new HashSet<Point>();
      var current = new HashSet<Point>();
      var next = new HashSet<Point>();

      current.Add(unvisited[0]);

      while (current.Count > 0) {
        foreach (Point point in current) {
          if (unvisited.Contains(point)) {
            unvisited.Remove(point);
          }

          var neighbors = new List<Point>();
          if (point.X > 0) {
            neighbors.Add(new Point(point.X - 1, point.Y));
          }

          if (point.Y > 0) {
            neighbors.Add(new Point(point.X, point.Y - 1));
          }

          if (point.X < 127) {
            neighbors.Add(new Point(point.X + 1, point.Y));
          }

          if (point.Y < 127) {
            neighbors.Add(new Point(point.X, point.Y + 1));
          }

          foreach (Point neighbor in neighbors) {
            if (grid[neighbor.X, neighbor.Y] && !seen.Contains(neighbor)) {
              next.Add(neighbor);
            }
          }
        }

        seen.UnionWith(current);
        current = next;
        next = new HashSet<Point>();
      }
    }

    Console.WriteLine("Regions: " + regions);
  }

  private static string KnotHash(string input) {
    const int LIST_SIZE = 256;
    int currentPosition = 0;
    int skipSize = 0;
    int[] list = new int[LIST_SIZE];
    for (int i = 0; i < LIST_SIZE; i++) {
      list[i] = i;
    }

    var lengths = new List<int>(input.Select(ch => (int)ch));
    lengths.AddRange(new[] { 17, 31, 73, 47, 23 });

    for (int round = 0; round < 64; round++) {
      foreach (int length in lengths) {
        // Reverse the order of length elements in the list, starting with the element at the current position.
        for (int i = 0; i < length / 2; i++) {
          // 0 1 2 3 4 5
          //   ^   x
          int idxA = (currentPosition + i) % LIST_SIZE;
          int idxB = ((currentPosition + length - i) - 1) % LIST_SIZE;
          int tmp = list[idxA];
          list[idxA] = list[idxB];
          list[idxB] = tmp;
        }

        // Move the current position forward by that length plus the skip size.
        // Increase the skip size by 1
        currentPosition += length + skipSize++;
      }
    }

    int BLOCKSIZE = 16;
    var hash = new StringBuilder();
    for (int i = 0; i < BLOCKSIZE; i++) {
      int val = list[i * BLOCKSIZE];

      for (int j = 1; j < BLOCKSIZE; j++) {
        val ^= list[i * BLOCKSIZE + j];
      }

      hash.Append(Convert.ToString(val, 2).PadLeft(8, '0'));
    }

    return hash.ToString();
  }
}
