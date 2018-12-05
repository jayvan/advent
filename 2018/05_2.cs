using System;
using System.Collections.Generic;

namespace advent2018 {
  public class Advent05_2 {
    public static void Main() {
      int min = Int32.MaxValue;
      string originalLine = Console.ReadLine();
      var letters = new HashSet<char>(originalLine.ToLower());

      foreach (char letter in letters) {
        string line = originalLine.Replace(letter.ToString(), string.Empty);
        line = line.Replace(char.ToUpper(letter).ToString(), string.Empty);

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

        min = Math.Min(min, line.Length);
      }

      Console.WriteLine(min);
    }
  }
}
