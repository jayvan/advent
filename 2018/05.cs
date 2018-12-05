using System;

namespace advent2018 {
  public class Advent05 {
    public static void Main() {
      string line = Console.ReadLine();

      bool reactionFound = true;

      while (reactionFound) {
        reactionFound = false;

        for (int i = 0; i < line.Length - 1; i++) {
          bool differentCase = char.IsUpper(line[i]) != char.IsUpper(line[i + 1]);
          bool sameLetter = char.ToLower(line[i]) == char.ToLower(line[i + 1]);
          if (differentCase && sameLetter) {
            line = line.Remove(i, 2);
            i--;
            reactionFound = true;
          }
        }
      }

      Console.WriteLine(line.Length);
    }
  }
}
