using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent15 {
  public static void Main () {
    var regex = new Regex(@"Disc #[0-9]+ has ([0-9]+) positions; at time=0, it is at position ([0-9]+)");
    var initialPositions = new List<int>();
    var numberOfPositions = new List<int>();

    string line;
    while ((line = Console.ReadLine()) != null) {
      Match match = regex.Match(line);
      numberOfPositions.Add(int.Parse(match.Groups[1].Value));
      initialPositions.Add(int.Parse(match.Groups[2].Value));
    }

    int dropTime = 0;

    while (true) {
      bool success = true;

      for (int i = 0; i < numberOfPositions.Count; i++) {
        if (((initialPositions[i] + dropTime + i + 1) % numberOfPositions[i]) != 0) {
          success = false;
          break;
        }
      }

      if (success) {
        Console.WriteLine(dropTime);
        return;
      }

      dropTime++;
    }
  }
}
