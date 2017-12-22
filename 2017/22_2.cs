using System;
using System.Collections.Generic;

public enum Direction { UP, DOWN, LEFT, RIGHT }

public static class DirectionExtensions {
  public static Direction Left (this Direction direction) {
    switch (direction) {
      case Direction.UP:
        return Direction.LEFT;
      case Direction.DOWN:
        return Direction.RIGHT;
      case Direction.LEFT:
        return Direction.DOWN;
      default:
        return Direction.UP;
    }
  }

  public static Direction Opposite (this Direction direction) {
    switch (direction) {
      case Direction.UP:
        return Direction.DOWN;
      case Direction.DOWN:
        return Direction.UP;
      case Direction.LEFT:
        return Direction.RIGHT;
      default:
        return Direction.LEFT;
    }
  }

  public static Direction Right (this Direction direction) {
    switch (direction) {
      case Direction.UP:
        return Direction.RIGHT;
      case Direction.DOWN:
        return Direction.LEFT;
      case Direction.LEFT:
        return Direction.UP;
      default:
        return Direction.DOWN;
    }
  }
}

public class Advent22 {
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

  private class Infection {
    enum CellState { CLEAN, WEAK, INFECTED, FLAGGED }

    public int InfectionsCaused { get; private set; }
    Dictionary<Point, CellState> locations = new Dictionary<Point, CellState>(20000000);
    private Point location;
    private Direction direction = Direction.UP;

    public void SetStart(int x, int y) {
      location = new Point(x, y);
    }

    public override string ToString() {
      return $"({location.X}, {location.Y}) {direction}";
    }

    public void Burst() {
      CellState state = GetState(location);
      if (state == CellState.CLEAN) {
        direction = direction.Left();
      } else if (state == CellState.INFECTED) {
        direction = direction.Right();
      } else if (state == CellState.FLAGGED) {
        direction = direction.Opposite();
      }

      SetState(location, (CellState)(((int)GetState(location) + 1) % 4));
      if (GetState(location) == CellState.INFECTED) {
        InfectionsCaused++;
      }

      location = location.Move(direction);
    }

    public void MarkInfection(int x, int y) {
      locations.Add(new Point(x, y), CellState.INFECTED);
    }

    private void SetState(Point p, CellState state) {
      locations.Remove(p);
      locations.Add(p, state);
    }

    private CellState GetState(Point p) {
      if (locations.ContainsKey(p)) {
        return locations[p];
      } else {
        return CellState.CLEAN;
      }
    }
  }

  public static void Main() {
    string line;
    int height = 0;
    int width = 0;
    var infection = new Infection();

    while ((line = Console.ReadLine()) != null) {
      width = line.Length;

      for (int x = 0; x < width; x++) {
        if (line[x] == '#') {
          infection.MarkInfection(x, height);
        }
      }

      height++;
    }

    infection.SetStart(width / 2, height / 2);

    for (int i = 0; i < 10000000; i++) {
      infection.Burst();
    }

    Console.WriteLine(infection.InfectionsCaused);
  }
}
