using System;
using System.Collections.Generic;
using System.Linq;

namespace advent2018 {
  public class Advent02 {
    public static void Main() {
      string line;
      var counts = new Dictionary<char, int>();
      int twos = 0;
      int threes = 0;

      while ((line = Console.ReadLine()) != null) {
        foreach (char c in line) {
          if (!counts.ContainsKey(c)) {
            counts.Add(c, 1);
          } else {
            counts[c] += 1;
          }
        }

        if (counts.Values.Contains(2)) {
          twos++;
        }

        if (counts.Values.Contains(3)) {
          threes++;
        }

        counts.Clear();
      }

      Console.WriteLine("Checksum: " + twos * threes);
    }
  }
}
