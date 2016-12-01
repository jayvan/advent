using System;
using System.Collections.Generic;

public class Advent01 {
  enum Direction {
    UP,
    RIGHT,
    DOWN,
    LEFT
  }
  public static void Main () {
    string[] commands = Console.ReadLine().Split(new [] {", "}, StringSplitOptions.None);
    Direction direction = Direction.UP;
    var visited = new HashSet<Tuple<int, int>>();
    visited.Add(new Tuple<int, int>(0, 0));
    bool doubledUp = false;
    int x = 0;
    int y = 0;

    foreach (string command in commands) {
      if (command[0] == 'R') {
        direction = (Direction)(((int)direction + 1) % 4);
      } else {
        direction = (Direction)(((int)direction + 3) % 4);
      }

      int distance = int.Parse(command.Substring(1));

      for (int j = 0; j < distance; j++) {
        switch (direction) {
          case Direction.UP:
            y++;
            break;
          case Direction.DOWN:
            y--;
            break;
          case Direction.LEFT:
            x--;
            break;
          case Direction.RIGHT:
            x++;
            break;
        }

        if (!doubledUp) {
          var location = new Tuple<int, int>(x, y);

          if (visited.Contains(location)) {
            Console.WriteLine("First Duplicate Distance: " + (Math.Abs(x) + Math.Abs(y)));
            doubledUp = true;
          } else {
            visited.Add(location);
          }
        }
      }
    }

    Console.WriteLine("Distance: " + (Math.Abs(x) + Math.Abs(y)));
  }
}
