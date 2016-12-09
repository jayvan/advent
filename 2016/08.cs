using System;
using System.Text.RegularExpressions;

public class Advent08 {
  public static void Main () {
    var rect = new Regex("rect ([0-9]+)x([0-9])+");
    var rotateRow = new Regex("rotate row y=([0-9]+) by ([0-9]+)");
    var rotateColumn = new Regex("rotate column x=([0-9]+) by ([0-9]+)");
    var grid = new bool[50, 6];
    var scratchRow = new bool[grid.GetLength(0)];
    var scratchColumn = new bool[grid.GetLength(1)];
    int shift = 0;
    int offset = 0;
    string line;

    while ((line = Console.ReadLine()) != null) {
      Match match = rect.Match(line);
      if (match.Success) {
        int width = int.Parse(match.Groups[1].Captures[0].ToString());
        int height = int.Parse(match.Groups[2].Captures[0].ToString());

        for (int x = 0; x < width; x++) {
          for (int y = 0; y < height; y++) {
            grid[x,y] = true;
          }
        }
      }

      match = rotateRow.Match(line);
      if (match.Success) {
        int row = int.Parse(match.Groups[1].Captures[0].ToString());
        shift = int.Parse(match.Groups[2].Captures[0].ToString());

        for (int i = 0; i < scratchRow.Length; i++) {
          offset = ((-shift + scratchRow.Length + i) % scratchRow.Length);
          scratchRow[i] = grid[offset, row];
        }

        for (int i = 0; i < scratchRow.Length; i++) {
          grid[i, row] = scratchRow[i];
        }
      }

      match = rotateColumn.Match(line);
      if (match.Success) {
        int column = int.Parse(match.Groups[1].Captures[0].ToString());
        shift = int.Parse(match.Groups[2].Captures[0].ToString());

        for (int i = 0; i < scratchColumn.Length; i++) {
          offset = ((-shift + scratchColumn.Length + i) % scratchColumn.Length);
          scratchColumn[i] = grid[column, offset];
        }

        for (int i = 0; i < scratchColumn.Length; i++) {
          grid[column, i] = scratchColumn[i];
        }
      }

    }
    int lightsOn = 0;

    for (int y = 0; y < grid.GetLength(1); y++) {
      for (int x = 0; x < grid.GetLength(0); x++) {
        if (grid[x, y]) {
          lightsOn++;
          Console.Write('#');
        } else {
          Console.Write('.');
        }
      }
      Console.Write("\n");
    }

    Console.WriteLine("Lights on: " + lightsOn);
  }
}
