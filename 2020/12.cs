using System;

public class Advent12 {
  private static int bearing;
  private static int[][] bearings = {
    new[] {1, 0},
    new[] {0, -1},
    new[] {-1, 0},
    new[] {0, 1}
  };

  public static void Main() {
    string line;

    int x = 0;
    int y = 0;

    while ((line = Console.ReadLine()) != null) {
      int magnitude = int.Parse(line.Substring(1));

      switch (line[0]) {
        case 'F':
          x += bearings[bearing][0] * magnitude;
          y += bearings[bearing][1] * magnitude;
          break;
        case 'L':
          bearing = ((bearing - magnitude / 90) % bearings.Length + bearings.Length) % bearings.Length;
          break;
        case 'R':
          bearing = (bearing + magnitude / 90) % bearings.Length;
          break;
        case 'N':
          y += magnitude;
          break;
        case 'S':
          y -= magnitude;
          break;
        case 'W':
          x -= magnitude;
          break;
        case 'E':
          x += magnitude;
          break;

      }
    }

    Console.WriteLine(Math.Abs(x) + Math.Abs(y));
  }
}
