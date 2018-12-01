using System;
using System.Collections.Generic;

public class Advent012 {
  public static void Main() {
    var deltas = new List<int>();
    string line;

    while ((line = Console.ReadLine()) != null) {
      deltas.Add(int.Parse(line));
    }

    var seen = new HashSet<int>();
    int index = 0;
    int frequency = 0;

    while (true) {
      frequency += deltas[index];
      if (seen.Contains(frequency)) {
        break;
      }

      seen.Add(frequency);
      index = (index + 1) % deltas.Count;
    }

    Console.WriteLine("First duplicate frequency: " + frequency);
  }
}
