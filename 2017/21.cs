using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Advent21 {
  public class Mapping {
    public readonly string[] Output;
    public readonly string Original;
    private HashSet<string> permutations = new HashSet<string>();

    public Mapping(string input, string output) {
      Output = output.Split('/');
      GenerateInput(input);
      Original = input;
    }

    public bool Matches(string block) {
      return permutations.Contains(block);
    }

    private void GenerateInput(string input) {
      for (int i = 0; i < 4; i++) {
        string rotated = Rotate(input);
        string flipped = Flip(input);

        permutations.Add(rotated);
        permutations.Add(flipped);
        input = rotated;
      }

      foreach (string perm in permutations) {
      }
    }

    private static string Flip(string contents) {
      string[] rows = contents.Split('/');
      var newRows = new string[rows.Length];

      for (int i = 0; i < rows.Length; i++) {
        char[] array = rows[i].ToCharArray();
        Array.Reverse(array);
        newRows[i] = new string(array);
      }

      return string.Join("/", newRows);
    }

    private static string Rotate(string contents) {
      var builder = new StringBuilder();
      string[] rows = contents.Split('/');
      var newRows = new string[rows.Length];

      for (int i = rows.Length - 1; i >= 0; i--) {
        builder.Clear();

        for (int j = 0; j < rows[0].Length; j++) {
          builder.Append(rows[j][i]);
        }

        newRows[rows.Length - 1 - i] = builder.ToString();
      }

      return string.Join("/", newRows);
    }
  }

  public static void Main() {
    // 101 is too low
    var builder = new StringBuilder();
    var mappings = new List<Mapping>();
    string line;

    while ((line = Console.ReadLine()) != null) {
      string[] parts = line.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
      mappings.Add(new Mapping(parts[0], parts[1]));
    }

    var grid = new [,] {
      { '#', '#', '#' },
      { '.', '.', '#' },
      { '.', '#', '.' }
    };

    for (int i = 0; i < 18; i++) {
      int size = grid.GetLength(0);
      int blockSize = size % 2 == 0 ? 2 : 3;
      int blocks = size / blockSize;
      int newSize = blocks * (blockSize + 1);
      int newBlockSize = blockSize + 1;

      var newGrid = new char[newSize, newSize];

      for (int blockY = 0; blockY < blocks; blockY++) {
        for (int blockX = 0; blockX < blocks; blockX++) {
          // Get the string for the source block
          builder.Clear();
          for (int y = 0; y < blockSize; y++) {
            for (int x = 0; x < blockSize; x++) {
              builder.Append(grid[blockY * blockSize + y, blockX * blockSize + x]);
            }

            if (y != blockSize - 1)
              builder.Append('/');
          }

          string block = builder.ToString();
          // Find the mapping
          var mapping = mappings.First(m => m.Matches(block));

          // insert the mapping output into the newGrid
          for (int y = 0; y < newBlockSize; y++) {
            for (int x = 0; x < newBlockSize; x++) {
              newGrid[blockY * newBlockSize + y, blockX * newBlockSize + x] = mapping.Output[y][x];
            }
          }
        }
      }

      grid = newGrid;
      Console.WriteLine("Iteration: " + i);
    }

    int count = 0;
    foreach (char c in grid) {
      if (c == '#') {
        count++;
      }
    }

    Console.WriteLine(count);
  }
}
