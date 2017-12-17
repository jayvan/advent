using System;
using System.Collections.Generic;

public class Advent17 {
  public static void Main() {
    int currentLocation = 0;
    var buffer = new List<int> { 0 };
    int stepSize = int.Parse(Console.ReadLine());

    for (int i = 1; i <= 2017; i++) {
      currentLocation = (currentLocation + stepSize) % buffer.Count + 1;
      buffer.Insert(currentLocation, i);
    }

    currentLocation = (currentLocation + 1) % buffer.Count;

    Console.WriteLine(buffer[currentLocation]);
  }
}
