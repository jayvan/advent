using System;
using System.Collections.Generic;

public struct Point {
  public readonly int X;
  public readonly int Y;

  public Point (int x, int y) {
    X = x;
    Y = y;
  }
}

public class Advent03 {
  enum Direction { Right, Up, Left, Down };

  public static void Main () {
    var grid = new Dictionary<Point, int>();
    grid.Add(new Point(0, 0), 1);

    int target = int.Parse(Console.ReadLine());

    int x = 0;
    int y = 0;

    int lineLength = 1;
    int linesDrawn = 0;
    int currentLineLength = 0;
    Direction direction = Direction.Right;

    while (true) {
      switch (direction) {
        case Direction.Right:
          x++;
          break;
        case Direction.Left:
          x--;
          break;
        case Direction.Up:
          y++;
          break;
        case Direction.Down:
          y--;
          break;
      }

      int newValue = 0;

      for (int i = x - 1; i <= x + 1; i++) {
        for (int j = y - 1; j <= y + 1; j++) {
          var adjacentLocation = new Point(i, j);
          if (grid.ContainsKey(adjacentLocation)) {
            newValue += grid[adjacentLocation];
          }
        }
      }

      if (newValue > target) {
        Console.WriteLine(newValue);
        return;
      }

      grid.Add(new Point(x, y), newValue);

      currentLineLength++;

      if (currentLineLength == lineLength) {
        currentLineLength = 0;
        linesDrawn++;
        direction = (Direction)(((int)direction + 1) % 4);

        if (linesDrawn == 2) {
          lineLength++;
          linesDrawn = 0;
        }
      }
    }
  }
}
