using System;
using System.Collections.Generic;
using System.Linq;

public class Advent06 {
  public static void Main () {
    string line = Console.ReadLine();

    var frequencies = new Dictionary<char, int>[line.Length];

    for (int i = 0; i < line.Length; i++) {
      frequencies[i] = new Dictionary<char, int>();
    }

   do {
      for (int i = 0; i < line.Length; i++) {
        Dictionary<char, int> slot = frequencies[i];
        char letter = line[i];

        if (slot.ContainsKey(letter)) {
          slot[letter] = slot[letter] + 1;
        } else {
          slot.Add(letter, 1);
        }
      }
    } while ((line = Console.ReadLine()) != null);

    Console.Write("Password: ");

    for (int i = 0; i < frequencies.Length; i++) {
      Console.Write((from entry in frequencies[i] orderby entry.Value descending select entry.Key).First());
    }

    Console.Write("\n");

    Console.Write("Alternate Password: ");

    for (int i = 0; i < frequencies.Length; i++) {
      Console.Write((from entry in frequencies[i] orderby entry.Value select entry.Key).First());
    }

    Console.Write("\n");
  }
}
