using System;

public class Advent02 {
  public static void Main () {
    string line;
    int checksum = 0;

    while ((line = Console.ReadLine()) != null) {
      int lowest = int.MaxValue;
      int highest = int.MinValue;

      foreach (string entry in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) {
        int num = int.Parse(entry);
        lowest = Math.Min(lowest, num);
        highest = Math.Max(highest, num);
      }

      checksum += highest - lowest;
    }

    Console.WriteLine($"Checksum: {checksum}");
  }
}
