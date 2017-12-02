using System;
using System.Linq;

public class Advent02 {
  public static void Main () {
    string line;
    int checksum = 0;

    while ((line = Console.ReadLine()) != null) {
      int[] entries = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
      checksum += DivisiblePairResult(entries);
    }

    Console.WriteLine($"Checksum: {checksum}");
  }

  private static int DivisiblePairResult(int[] entries) {
    Array.Sort(entries);
    for (int i = 0; i < entries.Length; i++) {
      for (int j = entries.Length - 1; j > i; j--) {
        if (entries[i] > entries[j] / 2) {
          break;
        }

        if (entries[j] % entries[i] == 0) {
          return entries[j] / entries[i];
        }
      }
    }

    throw new ArgumentException("The given entries do not have two values where one divides the other");
  }
}
