using System;

public class Advent11 {
  private static int SERIAL_NUMBER = 5235;

  public static void Main() {
    int[,] grid = new int[300, 300];
    int[,] cumulativeGrid = new int[300, 300];

    for (int y = 0; y < 300; y++) {
      for (int x = 0; x < 300; x++) {
        int rackId = x + 10;
        int power = rackId * y;
        power += SERIAL_NUMBER;
        power *= rackId;
        power = (power % 1000) / 100;
        power -= 5;
        grid[x, y] = power;
        cumulativeGrid[x, y] = power;
      }
    }

    int highest = int.MinValue;
    Tuple<int, int, int> location = null;

    for (int size = 2; size <= 300; size++) {
      Console.WriteLine("size: " + size);

      for (int x = 0; x < 300 - size; x++) {
        for (int y = 0; y < 300 - size; y++) {
          // Add the new column to the right of each
          for (int dy = y; dy < y + size; dy++) {
            cumulativeGrid[x, y] += grid[x + size - 1, dy];
          }

          // Add the new row to the bottom
          for (int dx = x; dx < x + size - 1; dx++) {
            cumulativeGrid[x, y] += grid[dx, y + size - 1];
          }

          if (cumulativeGrid[x, y] > highest) {
            highest = cumulativeGrid[x, y];
            location = new Tuple<int, int, int>(x, y, size);
          }
        }
      }
    }

    Console.WriteLine($"Highest power is {highest} at {location}");
  }
}
