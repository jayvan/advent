using System;

public class Advent122 {
  public static void Main() {
    string line;

    int x = 0;
    int y = 0;
    int wayX = 10;
    int wayY = 1;

    while ((line = Console.ReadLine()) != null) {
      int magnitude = int.Parse(line.Substring(1));
      int tmp;

      switch (line[0]) {
        case 'F':
          x += wayX * magnitude;
          y += wayY * magnitude;
          break;
        case 'L':
          for (int i = 0; i < magnitude; i += 90) {
            tmp = -wayY;
            wayY = wayX;
            wayX = tmp;
          }
          break;
        case 'R':
          for (int i = 0; i < magnitude; i += 90) {
            tmp = -wayX;
            wayX = wayY;
            wayY = tmp;
          }

          break;
        case 'N':
          wayY += magnitude;
          break;
        case 'S':
          wayY -= magnitude;
          break;
        case 'W':
          wayX -= magnitude;
          break;
        case 'E':
          wayX += magnitude;
          break;

      }
    }

    Console.WriteLine(Math.Abs(x) + Math.Abs(y));
  }
}
