using System;
using System.Collections.Generic;

public class Advent19 {
  public static void Main () {
    int elfCount = int.Parse(Console.ReadLine());

    var elves = new bool[elfCount];

    int elfIndex = 0;

    while (elfCount > 1) {
      int nextElfIndex = (elfIndex + 1) % elves.Length;
      nextElfIndex = Array.IndexOf(elves, false, nextElfIndex);

      if (nextElfIndex == -1) {
        nextElfIndex = Array.IndexOf(elves, false);
      }

      elves[nextElfIndex] = true;
      elfCount--;

      elfIndex = Array.IndexOf(elves, false, nextElfIndex);
      if (elfIndex == -1) {
        elfIndex = Array.IndexOf(elves, false);
      }
    }

    Console.WriteLine("Last Elf standing: " + (elfIndex + 1));
  }
}
