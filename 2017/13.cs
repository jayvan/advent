using System;
using System.Collections.Generic;

public class Advent13 {
  public static void Main() {
    var splitString = new[] { ": " };
    var layerHeights = new Dictionary<int, int>();
    int lastLayer = 0;
    int severity = 0;

    string line;
    while ((line = Console.ReadLine()) != null) {
      string[] values = line.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
      int layer = int.Parse(values[0]);
      int layerHeight = int.Parse(values[1]);
      layerHeights.Add(layer, layerHeight);
      lastLayer = layer;
    }

    for (int i = 0; i <= lastLayer; i++) {
      if (!layerHeights.ContainsKey(i)) {
        continue;
      }

      int height = layerHeights[i];
      int period = 2 * height - 2;
      int phase = i % period;

      int location = phase;
      if (location >= height) {
        location = period - phase;
      }

      if (location == 0) {
        severity += i * height;
      }
    }

    Console.WriteLine("Severity: " + severity);
  }
}
