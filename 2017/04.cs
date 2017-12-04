using System;
using System.Collections.Generic;

public class Advent04 {

  public static void Main () {
    int validCount = 0;
    string line;

    while ((line = Console.ReadLine()) != null) {
      string[] words = line.Split(' ');
      var uniqueWords = new HashSet<string>(words);
      if (uniqueWords.Count == words.Length) {
        validCount++;
      }
    }

    Console.WriteLine($"Valid Passwords: {validCount}");
  }
}
