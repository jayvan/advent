using System;
using System.Collections.Generic;

public class Advent15 {
  public static void Main() {
    int turn = 0;
    var lastCall = new Dictionary<int, int>();
    string line;
    string[] starting = Console.ReadLine().Split(',');

    for (int i = 0; i < starting.Length - 1; i++) {
      lastCall[int.Parse(starting[i])] = turn;
      Console.WriteLine($"Turn {turn + 1}: {starting[i]}");
      turn++;
    }

    int currentNum = int.Parse(starting[starting.Length - 1]);

    while (turn < 30000000) {
      int nextNum;

      if (lastCall.ContainsKey(currentNum)) {
        nextNum = turn - lastCall[currentNum];
      } else {
        nextNum = 0;
      }

      lastCall[currentNum] = turn;
      if (turn > 29999990)
      Console.WriteLine($"Turn {turn + 1}: {currentNum}");
      currentNum = nextNum;
      turn++;
    }
  }
}
