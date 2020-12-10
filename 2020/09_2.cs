using System;
using System.Collections.Generic;

public class Advent092 {
  public static void Main() {
    string line;
    List<long> list = new List<long>();

    const long ANSWER = 29221323;

    while ((line = Console.ReadLine()) != null) {
      list.Add(long.Parse(line));
    }

    int minIndex = 0;
    int maxIndex = 1;
    long sum = list[0] + list[1];

    while (true) {
      if (sum < ANSWER) {
        maxIndex++;
        sum += list[maxIndex];
      } else if (sum > ANSWER) {
        sum -= list[minIndex];
        minIndex++;
      }

      if (sum == ANSWER) {
        break;
      }
    }

    long min = list[minIndex];
    long max = list[maxIndex];
    Console.WriteLine($"{min} - {max}");

    for (int i = minIndex; i <= maxIndex; i++) {
      min = Math.Min(min, list[i]);
      max = Math.Max(max, list[i]);
    }

    Console.WriteLine(min + max);
  }
}
