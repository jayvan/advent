using System;
using System.Collections.Generic;
using System.Linq;

public class Advent06 {
  public static void Main () {
    int[] slots = Console.ReadLine()
                         .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(i => int.Parse(i)).ToArray();

    var visited = new Dictionary<string, int>();

    int iterations = 0;
    string hash = string.Empty;

    while (true) {
      hash = string.Join(" ", slots);

      if (visited.ContainsKey(hash)) {
        break;
      }

      visited.Add(hash, iterations);

      int highestIndex = 0;
      int highestValue = 0;

      for (int i = 0; i < slots.Length; i++) {
        if (slots[i] > highestValue) {
          highestValue = slots[i];
          highestIndex = i;
        }
      }

      slots[highestIndex] = 0;

      int amountPerSlot = highestValue / slots.Length;
      int slotsWithOneExtra = highestValue % slots.Length;

      for (int i = 0; i < slots.Length; i++) {
        int indexToAdd = (highestIndex + i + 1) % slots.Length;

        slots[indexToAdd] += amountPerSlot;

        if (i < slotsWithOneExtra) {
          slots[indexToAdd]++;
        }
      }

      iterations++;
    }

    int loopSize = iterations - visited[hash];
    Console.WriteLine($"Iterations: {visited.Count()}, loop size: {loopSize}");
  }
}
