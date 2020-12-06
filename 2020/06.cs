using System;
using System.Collections.Generic;

public class Advent06 {
  public static void Main() {
    HashSet<char> answers = new HashSet<char>();
    int sum = 0;

    string line;
    while (true) {
      line = Console.ReadLine();

      if (string.IsNullOrEmpty(line)) {
        sum += answers.Count;
        answers.Clear();

        if (line == null) {
          break;
        }

        continue;

      }

      foreach (char t in line) {
        answers.Add(t);
      }
    }

    Console.WriteLine(sum);
  }
}
