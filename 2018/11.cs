using System;

public class Advent11 {
  private static int SERIAL_NUMBER = 5235;

  public static void Main() {
    int[,] grid = new int[300, 300];

    for (int y = 0; y < 300; y++) {
      for (int x = 0; x < 300; x++) {
        int rackId = x + 10;
        int power = rackId * y;
        power += SERIAL_NUMBER;
        power *= rackId;
        power = (power % 1000) / 100;
        power -= 5;

        for (int xd = x - 1; xd <= x + 1; xd++) {
          for (int yd = y - 1; yd <= y + 1; yd++) {
            if (xd >= 0 && yd >= 0 && xd < 300 && yd < 300) {
              grid[xd, yd] += power;
            }
          }
        }
      }
    }

    int highest = int.MinValue;
    Tuple<int, int> location = null;
    for (int y = 0; y < 300; y++) {
      for (int x = 0; x < 300; x++) {
        if (grid[x, y] > highest) {
          highest = grid[x, y];
          location = new Tuple<int, int>(x - 1, y - 1);
        }
      }
    }

    Console.WriteLine($"Highest power is {highest} at {location}");
  }
}
