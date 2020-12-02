using System;
using System.Text.RegularExpressions;

public class Advent022 {
  public static void Main () {
    Regex regex = new Regex(@"([0-9]+)-([0-9]+) ([a-z]): ([a-z]+)", RegexOptions.IgnoreCase);
    string line;
    int valid = 0;

    while ((line = Console.ReadLine()) != null) {
      Match match = regex.Match(line);

      int a = int.Parse(match.Groups[1].Captures[0].Value) - 1;
      int b = int.Parse(match.Groups[2].Captures[0].Value) - 1;
      char c = match.Groups[3].Captures[0].Value[0];
      string password = match.Groups[4].Captures[0].Value;

      if ((password[a] == c) ^ (password[b] == c)) {
        valid++;
      }
    }
      Console.WriteLine(valid);
  }
}
