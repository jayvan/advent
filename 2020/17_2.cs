using System;
using System.Collections.Generic;

public class Advent172 {
  static HashSet<ValueTuple<int, int, int, int>> enabled = new HashSet<(int, int, int, int)>();

  public static void Main() {
    string line;
    int y = 0;

    while ((line = Console.ReadLine()) != null) {
      for (int x = 0; x < line.Length; x++) {
        if (line[x] == '#') {
          enabled.Add((x, y, 0, 0));
        }
      }

      y++;
    }

    Console.WriteLine(enabled.Count);
    for (int i = 0; i < 6; i++) {
      Tick();
      Console.WriteLine(enabled.Count);
    }
  }

  private static void Tick() {
    var counts = new Dictionary<(int, int, int, int), int>();

    foreach ((int, int, int, int) valueTuple in enabled) {
      if (!counts.ContainsKey(valueTuple)) {
        counts.Add(valueTuple, 0);
      }

      for (int x = valueTuple.Item1 - 1; x <= valueTuple.Item1 + 1; x++) {
        for (int y = valueTuple.Item2 - 1; y <= valueTuple.Item2 + 1; y++) {
          for (int z = valueTuple.Item3 - 1; z <= valueTuple.Item3 + 1; z++) {
            for (int w = valueTuple.Item4 - 1; w <= valueTuple.Item4 + 1; w++) {
              (int, int, int, int) cube = (x, y, z, w);

              if (cube.Equals(valueTuple)) {
                continue;
              }

              if (counts.ContainsKey(cube)) {
                counts[cube]++;
              } else {
                counts[cube] = 1;
              }
            }
          }
        }
      }
    }

    var newEnabled = new HashSet<(int, int, int, int)>();

    foreach (KeyValuePair<(int, int, int, int), int> kvp in counts) {
      bool isOn = (enabled.Contains(kvp.Key) && (kvp.Value == 2 || kvp.Value == 3)) ||
                  (!enabled.Contains(kvp.Key) && kvp.Value == 3);


      if (isOn) {
        newEnabled.Add(kvp.Key);
      }
    }

    enabled = newEnabled;
  }
}
