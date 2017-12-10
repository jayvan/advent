using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Advent09 {
  public static void Main () {
    string line;

    while ((line = Console.ReadLine()) != null) {
      ScoreStream(line);
    }
  }

  private static void ScoreStream(string stream) {
    int score = 0;
    int groupDepth = 0;
    int garbageLength = 0;
    bool inGarbage = false;
    // '{' represents the start of a *group*
    // '}' represents the end of a *group*
    // '<' enters *garbage* mode, where everything but a '>' is ignored
    // The exception is that in garbage mode a '!' skips the next char

    for (int i = 0; i < stream.Length; i++) {
      if (inGarbage) {
        if (stream[i] == '!') {
          i++;
        } else if (stream[i] == '>') {
          inGarbage = false;
        } else {
          garbageLength++;
        }
      } else {
        if (stream[i] == '<') {
          inGarbage = true;
        } else if (stream[i] == '{') {
          groupDepth++;
        } else if (stream[i] == '}') {
          score += groupDepth--;
        }
      }
    }

    Console.WriteLine("Score was " + score);
    Console.WriteLine("Garbage amount: " + garbageLength);
  }
}
