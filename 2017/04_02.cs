using System;
using System.Collections.Generic;

public class Advent04 {

  public static void Main () {
    int validCount = 0;
    string line;

    while ((line = Console.ReadLine()) != null) {
      string[] words = line.Split(' ');
      var uniqueWords = new HashSet<string>();

      bool valid = true;
      foreach (string word in words) {
        char[] wordArray = word.ToCharArray();
        Array.Sort(wordArray);
        string sortedWord = new String(wordArray);

        if (uniqueWords.Contains(sortedWord)) {
          valid = false;
          break;
        }

        uniqueWords.Add(sortedWord);
      }


      if (valid) {
        validCount++;
      }
    }

    Console.WriteLine($"Valid Passwords: {validCount}");
  }
}
