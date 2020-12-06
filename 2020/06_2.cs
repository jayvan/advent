using System;
using System.Collections.Generic;
using System.Linq;

public class Advent062 {

  public static void Main() {
    HashSet<char> answers = new HashSet<char>();
    bool newGroup = true;
    int sum = 0;

    string line;
    while (true) {
      line = Console.ReadLine();

      if (string.IsNullOrEmpty(line)) {
        sum += answers.Count;
        answers.Clear();
        newGroup = true;

        if (line == null) {
          break;
        }

        continue;

      }

      if (newGroup) {
        foreach (char t in line) {
          answers.Add(t);
        }
      } else {
        foreach (char c in answers.ToArray()) {
          if (!line.Contains(c)) {
            answers.Remove(c);
          }
        }
      }

      newGroup = false;
    }

    Console.WriteLine(sum);
  }
}
