using System;
using System.Collections.Generic;

public class Advent09 {
  public static void Main() {
    string line;

    const int PREAMBLE = 25;
    List<int> recent = new List<int>(PREAMBLE);
    HashSet<int> pairings = new HashSet<int>();

    for (int i = 0; i < PREAMBLE; i++) {
      recent.Add(int.Parse(Console.ReadLine()));
    }

    while ((line = Console.ReadLine()) != null) {
      int current = int.Parse(line);

      bool found = false;

      for (int i = 0; i < PREAMBLE; i++) {
        if (pairings.Contains(recent[i])) {
          found = true;
          break;
        }

        pairings.Add(current - recent[i]);
      }

      if (!found) {
        Console.WriteLine(current);
        break;
      }

      recent.RemoveAt(0);
      recent.Add(current);
      pairings.Clear();
    }
  }

}
