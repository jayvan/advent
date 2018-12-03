using System;
using System.Collections.Generic;

namespace advent2018 {
  public static class Advent02_2 {
    public static void Main() {
      string line;
      var lines = new List<string>();

      while ((line = Console.ReadLine()) != null) {
        lines.Add(line);
      }

      for (int a = 0; a < lines.Count - 2; a++) {
        for (int b = a + 1; b < lines.Count; b++) {
          int differingIndex = DifferingIndex(lines[a], lines[b]);

          if (differingIndex >= 0) {
            Console.WriteLine(lines[a].Remove(differingIndex, 1));
            Environment.Exit(0);
          }
        }
      }

      Console.WriteLine("Didn't find similar bins");
    }

    public static int DifferingIndex(string a, string b) {
      int index = -1;

      for (int i = 0; i < a.Length; i++) {
        if (a[i] != b[i]) {
          if (index == -1) {
            index = i;
          } else {
            return -1;
          }
        }
      }
      return index;
    }
  }
}
