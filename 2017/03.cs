using System;

public class Advent03 {
  enum Direction { Right, Up, Left, Down };

  public static void Main () {
    int target = int.Parse(Console.ReadLine());
    int count = 1;

    int x = 0;
    int y = 0;

    int lineLength = 1;
    int linesDrawn = 0;
    int currentLineLength = 0;
    Direction direction = Direction.Right;

    while (count < target) {
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

      count++;
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

    int distance = Math.Abs(x) + Math.Abs(y);
    Console.WriteLine($"Distance: {distance}");
  }
}
