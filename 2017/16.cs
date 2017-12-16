using System;
using System.Collections.Generic;
using System.Linq;

public class Advent16 {
  public static void Main() {
    const int PROGRAM_COUNT = 16;
    const int RUNS = 1000000000;
    var line = new char[PROGRAM_COUNT];
    var newLine = new char[PROGRAM_COUNT];

    for (int i = 0; i < PROGRAM_COUNT; i++) {
      line[i] = (char)((int)'a' + i);
    }

    string input = Console.ReadLine();
    string[] danceMoves = input.Split(',');
    var cycleDetector = new Dictionary<string, int>();
    for (int j = 0; j < RUNS; j++) {
      foreach (string move in danceMoves) {
        if (move[0] == 's') {
          int spinCount = int.Parse(move.Substring(1));
          Array.Copy(line, PROGRAM_COUNT - spinCount, newLine, 0, spinCount);
          Array.Copy(line, 0, newLine, spinCount, PROGRAM_COUNT - spinCount);
          char[] tmp = line;
          line = newLine;
          newLine = tmp;
        } else if (move[0] == 'x') {
          int[] indices = move.Substring(1).Split('/').Select(i => int.Parse(i)).ToArray();
          char tmp = line[indices[0]];
          line[indices[0]] = line[indices[1]];
          line[indices[1]] = tmp;
        } else if (move[0] == 'p') {
          char[] myLine = line;
          int[] indices = move.Substring(1).Split('/').Select(s => Array.IndexOf(myLine, s[0])).ToArray();
          char tmp = line[indices[0]];
          line[indices[0]] = line[indices[1]];
          line[indices[1]] = tmp;
        }
      }

      string result = new string(line);

      if (cycleDetector.ContainsKey(result)) {
        int lengthOfCycle = j - cycleDetector[result];
        j += ((RUNS - j) / lengthOfCycle) * lengthOfCycle;
      } else {
        cycleDetector.Add(result, j);
      }
    }

    foreach (char c in line) {
      Console.Write(c);
    }
    Console.WriteLine();
  }
}
