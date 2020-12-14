using System;
using System.Collections.Generic;

public class Advent10 {
  public static void Main() {
    string line;
    List<int> jolts = new List<int>();
    jolts.Add(0);

    while ((line = Console.ReadLine()) != null) {
      jolts.Add(int.Parse(line));
    }

    jolts.Sort();

    jolts.Add(jolts[jolts.Count - 1] + 3);

    int joltage = 0;
    int oneJumps = 0;
    int threeJumps = 0;

    for (int i = 0; i < jolts.Count; i++) {
      int newJoltage = jolts[i];
      int diff = newJoltage - joltage;

      if (diff == 1) {
        oneJumps++;
      } else if (diff == 3) {
        threeJumps++;
      }

      joltage = newJoltage;
    }

    Console.WriteLine(oneJumps * threeJumps);

    long[] combinations = new long[jolts.Count];
    combinations[jolts.Count - 1] = 1;

    for (int i = jolts.Count - 2; i >= 0; i--) {
      for (int j = i + 1; j < Math.Min(jolts.Count, i + 4); j++) {
//        Console.WriteLine($"{jolts[i]} -> {jolts[j]}");

        if (jolts[j] - jolts[i] <= 3) {
          combinations[i] += combinations[j];
//          Console.WriteLine($"{combinations[i]}");
        }
      }
    }

    Console.WriteLine("---");
    for (int i = 0; i < combinations.Length; i++) {
       Console.WriteLine(combinations[i]);
    }
  }
}
