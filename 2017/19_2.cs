using System;
using System.Collections.Generic;
using System.Text;

public enum Direction { UP, DOWN, LEFT, RIGHT, NONE }

public static class DirectionExtensions {
  public static Direction Opposite (this Direction direction) {
    switch (direction) {
      case Direction.UP:
        return Direction.DOWN;
      case Direction.DOWN:
        return Direction.UP;
      case Direction.LEFT:
        return Direction.RIGHT;
      case Direction.RIGHT:
        return Direction.LEFT;
    }

    return Direction.NONE;
  }
}

public class Network {


  private struct Point {
    public readonly int X;
    public readonly int Y;

    public Point(int x, int y) {
      X = x;
      Y = y;
    }

    public Point Move(Direction direction) {
      if (direction == Direction.UP) {
        return new Point(this.X, this.Y - 1);
      }  else if (direction == Direction.DOWN) {
        return new Point(this.X, this.Y + 1);
      } else if (direction == Direction.LEFT) {
        return new Point(this.X - 1, this.Y);
      } else if (direction == Direction.RIGHT) {
        return new Point(this.X + 1, this.Y);
      }

      return new Point(this.X, this.Y);
    }
  }

  private List<string> grid;

  public Network(List<string> grid) {
    this.grid = grid;
  }

  public int Path() {
    var location = new Point(grid[0].IndexOf('|'), 0);
    var direction = Direction.DOWN;
    int stepCount = 0;

    while (true) {
      Console.WriteLine($"({location.X}, {location.Y}) {direction}");
      location = location.Move(direction);
      stepCount++;
      char stop = CharAt(location);

      if (stop == '+') {
        direction = NextDirection(location, direction);
      }

      if (direction == Direction.NONE || !Pathable(location)) {
        return stepCount;
      }

    }
  }

  private Direction NextDirection(Point location, Direction current) {
    foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
      if (direction != current && direction != current.Opposite() && CharAt(location.Move(direction)) != ' ') {
        return direction;
      }
    }

    return Direction.NONE;
  }

  private char CharAt(Point point) {
    if (Pathable(point)) {
      return grid[point.Y][point.X];
    }

    return ' ';
  }

  private bool Pathable(Point point) {
    return point.Y >= 0 && point.Y < grid.Count && point.X >= 0 && point.X < grid[point.Y].Length && grid[point.Y][point.X] != ' ';
  }
}
public class Advent19 {

  public static void Main() {
    var grid = new List<string>();

    string line;
    while ((line = Console.ReadLine()) != null) {
      grid.Add(line);
    }

    var network = new Network(grid);
    Console.WriteLine(network.Path());
  }
}
