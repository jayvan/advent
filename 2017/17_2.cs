using System;
using System.Collections.Generic;

public class Advent17 {
  public static void Main() {
    int currentLocation = 0;
    int stepSize = int.Parse(Console.ReadLine());
    int afterZero = 0;

    for (int i = 1; i <= 50000000; i++) {
      currentLocation = (currentLocation + stepSize) % i + 1;
      if (currentLocation == 1) {
        afterZero = i;
      }
    }

    Console.WriteLine(afterZero);
  }
}
