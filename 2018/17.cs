using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Advent17 {
  public static void Main() {
    var sim = new WaterSim();

    string line;

    while ((line = Console.ReadLine()) != null) {
      if (line[0] == 'x') {
        int x = int.Parse(line.Split(',')[0].Split('=')[1]);
        int[] ys = line.Split(' ')[1].Substring(2).Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Select(a => int.Parse(a)).ToArray();

        for (int y = ys[0]; y <= ys[1]; y++) {
          sim.AddClay(x, y);
        }
      } else {
        int y = int.Parse(line.Split(',')[0].Split('=')[1]);
        int[] xs = line.Split(' ')[1].Substring(2).Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Select(a => int.Parse(a)).ToArray();

        for (int x = xs[0]; x <= xs[1]; x++) {
          sim.AddClay(x, y);
        }
      }
    }

    sim.Simulate();
    Console.WriteLine(sim.VisitCount);
    Console.WriteLine(sim.WaterCount);
  }

  public class WaterSim {
    private HashSet<Tuple<int, int>> Clay = new HashSet<Tuple<int, int>>();
    private HashSet<Tuple<int, int>> Visited = new HashSet<Tuple<int, int>>();
    private HashSet<Tuple<int, int>> Water = new HashSet<Tuple<int, int>>();
    private int minY = int.MaxValue;
    private int maxY = int.MinValue;
    private int minX = int.MaxValue;
    private int maxX = int.MinValue;

    public int VisitCount => this.Visited.Count - 1;
    public int WaterCount => this.Water.Count;

    public void AddClay(int x, int y) {
      Clay.Add(new Tuple<int, int>(x, y));
      minY = Math.Min(minY, y);
      maxY = Math.Max(maxY, y);
      this.minX = Math.Min(this.minX, x - 1);
      this.maxX = Math.Max(this.maxX, x + 1);
    }

    public void Simulate() {
      HashSet<Tuple<int, int>> blocked = new HashSet<Tuple<int, int>>(Clay);
      Stack<Tuple<int, int>> todo = new Stack<Tuple<int, int>>();
      todo.Push(new Tuple<int, int>(500, this.minY - 1));

      int iterations = 0;
      while (todo.Count > 0) {
        iterations++;
        Tuple<int, int> point = todo.Pop();
        if (iterations % 10000 == 0) {
          Console.WriteLine($"Depth: {point.Item2}, Visited: {VisitCount}, Water: {WaterCount}");
        }

        Visited.Add(point);
        // Go down as far as possible, enqueueing side checks along the way
        int y;
        bool hitFloor = false;
        for (y = point.Item2 + 1; y <= this.maxY; y++) {
          var location = new Tuple<int, int>(point.Item1, y);
          if (blocked.Contains(location)) {
            hitFloor = true;
            y--;
            break;
          }
          Visited.Add(location);
        }

        if (!hitFloor) {
          continue;
        }

        int leftX = point.Item1;
        bool leftWall = false;
        while(leftX >= this.minX) {
          leftWall = blocked.Contains(new Tuple<int, int>(leftX - 1, y));
          bool hasFloor = blocked.Contains(new Tuple<int, int>(leftX, y + 1));

          if (!hasFloor) {
            todo.Push(new Tuple<int, int>(leftX, y));
            break;
          }

          if (leftWall) {
            break;
          }
          leftX--;
        }

        int rightX = point.Item1;
        bool rightWall = false;
        while (rightX <= this.maxX) {
          rightWall = blocked.Contains(new Tuple<int, int>(rightX + 1, y));
          bool hasFloor = blocked.Contains(new Tuple<int, int>(rightX, y + 1));

          if (!hasFloor) {
            todo.Push(new Tuple<int, int>(rightX, y));
            break;
          }

          if (rightWall) {
            break;
          }

          rightX++;
        }

        for (int x = leftX; x <= rightX; x++) {
          var location = new Tuple<int, int>(x, y);
          this.Visited.Add(location);

          if (leftWall && rightWall) {
            blocked.Add(location);
            this.Water.Add(location);
          }
        }

        if (leftWall && rightWall) {
          todo.Push(new Tuple<int, int>(point.Item1, y - 1));
        }
      }
    }

    public void Draw() {
      for (int y = this.minY; y <= this.maxY; y++) {
        for (int x = this.minX; x <= this.maxX; x++) {
          var location = new Tuple<int, int>(x, y);
          if (this.Clay.Contains(location)) {
            Console.Write('#');
          } else if (this.Water.Contains(location)) {
            Console.Write('~');
          } else if (this.Visited.Contains(location)) {
            Console.Write('|');
          } else {
            Console.Write('.');
          }
        }
        Console.WriteLine();
      }

      Console.WriteLine("\n\n");
    }
  }
}
