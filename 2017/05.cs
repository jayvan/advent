using System;
using System.Collections.Generic;

public class Advent05 {
  public static void Main () {
    int index = 0;
    int jumps = 0;
    var offsets = new List<int>();

    string line;

    while ((line = Console.ReadLine()) != null) {
      offsets.Add(int.Parse(line));
    }

    while (index >= 0 && index < offsets.Count) {
      index += offsets[index]++;
      jumps++;
    }

    Console.WriteLine($"Jumps taken: {jumps}");
  }
}
