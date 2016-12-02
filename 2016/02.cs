using System;
using System.Collections.Generic;

public class Advent02 {
  public static void Main () {
    var directions = new List<string>();

    string line;
    while ((line = Console.ReadLine()) != null) {
      directions.Add(line);
    }

    var gridA = new char[,] {{ '1', '2', '3' },
                             { '4', '5', '6' },
                             { '7', '8', '9' }};

    var gridB = new char[,] {{ ' ', ' ', '1', ' ', ' ' },
                             { ' ', '2', '3', '4', ' ' },
                             { '5', '6', '7', '8', '9' },
                             { ' ', 'A', 'B', 'C', ' ' },
                             { ' ', ' ', 'D', ' ', ' ' }};

    string codeA = GetCode(gridA, directions, 1, 1);
    string codeB = GetCode(gridB, directions, 0, 2);
    Console.WriteLine("Part One: " + codeA);
    Console.WriteLine("Part Two: " + codeB);
  }

  private static string GetCode(char[,] grid, IEnumerable<string> directions, int x, int y) {
    string code = "";

    foreach (string steps in directions) {
      foreach (char direction in steps) {
        if (direction == 'U' && y > 0 && grid[y - 1, x] != ' ') {
          y--;
        } else if (direction == 'L' && x > 0 && grid[y, x - 1] != ' ') {
          x--;
        } else if (direction == 'R' && x < grid.GetLength(1) - 1 && grid[y, x + 1] != ' ') {
          x++;
        } else if (direction == 'D' && y < grid.GetLength(0) - 1 && grid[y + 1, x] != ' ') {
          y++;
        }
      }

      code += grid[y, x];
    }

    return code;
  }
}
