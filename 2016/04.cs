using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Advent04 {
  public static void Main () {
    Regex regex = new Regex(@"([a-z\-]+)([0-9]+)\[([a-z]+)\]", RegexOptions.IgnoreCase);
    int sectorSum = 0;
    string line;

    while ((line = Console.ReadLine()) != null) {
      Match match = regex.Match(line);
      string code = match.Groups[1].Captures[0].ToString();
      string checksum = match.Groups[3].Captures[0].ToString();
      int sectorId = int.Parse(match.Groups[2].Captures[0].ToString());
      var charCount = new Dictionary<char, int>();
      Console.Write(sectorId + ": ");

      foreach (char c in code) {
        if (c == '-') {
          continue;
        }

        Console.Write((char)(((((int)c - 97) + sectorId) % 26) + 97));

        if (!charCount.ContainsKey(c)) {
          charCount.Add(c, 1);
        } else {
          charCount[c] = charCount[c] + 1;
        }
      }
      Console.Write("\n");

      string mostCommon = new String((from entry in charCount orderby entry.Value descending, entry.Key ascending select entry.Key).Take(5).ToArray());

      if (mostCommon == checksum) {
        sectorSum += sectorId;
      }
    }

    Console.WriteLine("Sum of valid sectors: " + sectorSum);
  }
}
