using System;

public class Advent01 {
  public static void Main () {
    bool[] seen = new bool[2020];
    string line;

    while ((line = Console.ReadLine()) != null) {
      int num = int.Parse(line);

      if (num >= 2020) {
        continue;
      }

      if (seen[2020 - num]) {
        Console.WriteLine($"{num} + {2020 - num} = {num * (2020 - num)}");
      }

      seen[num] = true;
    }
  }
}
