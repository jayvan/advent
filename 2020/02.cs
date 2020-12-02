using System;
using System.Text.RegularExpressions;

public class Advent02 {
  public static void Main () {
    Regex regex = new Regex(@"([0-9]+)-([0-9]+) ([a-z]): ([a-z]+)", RegexOptions.IgnoreCase);
    string line;
    int valid = 0;

    while ((line = Console.ReadLine()) != null) {
      Match match = regex.Match(line);

      int min = int.Parse(match.Groups[1].Captures[0].Value);
      int max = int.Parse(match.Groups[2].Captures[0].Value);
      char c = match.Groups[3].Captures[0].Value[0];
      int count = Array.FindAll(match.Groups[4].Captures[0].Value.ToCharArray(), a => a == c).Length;
      if (min <= count && count <= max) {
        valid++;
      }
    }
      Console.WriteLine(valid);
  }
}
