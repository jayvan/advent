using System;
using System.Collections.Generic;

public class Advent13 {
  public static void Main() {
    var splitString = new[] { ": " };
    var layerHeights = new Dictionary<int, int>();
    int lastLayer = 0;

    string line;
    while ((line = Console.ReadLine()) != null) {
      string[] values = line.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
      int layer = int.Parse(values[0]);
      int layerHeight = int.Parse(values[1]);
      layerHeights.Add(layer, layerHeight);
      lastLayer = layer;
    }

    int pause = 0;
    while (true) {
      if (IsSafe(layerHeights, lastLayer, pause)) {
        Console.WriteLine("Wait Time: " + pause);
        break;
      }

      pause++;
    }
  }

  private static bool IsSafe(Dictionary<int, int> heights, int maxHeight, int pause) {
    for (int i = 0; i <= maxHeight; i++) {
      if (!heights.ContainsKey(i)) {
        continue;
      }

      int height = heights[i];
      int period = 2 * height - 2;
      int phase = (i + pause) % period;

      int location = phase;
      if (location >= height) {
        location = period - phase;
      }

      if (location == 0) {
        return false;
      }
    }

    return true;

  }
}
