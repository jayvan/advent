using System;
using System.Collections.Generic;

public class Advent12 {
  public static void Main () {
    var graph = new Dictionary<string, string[]>();
    string line;

    string[] partSplit = new[] { " <-> " };
    string[] neighborSplit = new[] { ", " };

    var programs = new Dictionary<string, bool>();

    while ((line = Console.ReadLine()) != null) {
      string[] parts = line.Split(partSplit, StringSplitOptions.RemoveEmptyEntries);
      // 2 <-> 0, 3, 4
      graph.Add(parts[0], parts[1].Split(neighborSplit, StringSplitOptions.RemoveEmptyEntries));
      programs.Add(parts[0], true);
    }

    int groups = 0;

    while (programs.Count > 0) {
      groups++;
      var seen = new HashSet<string>();
      var current = new HashSet<string>();
      var next = new HashSet<string>();

      var keyEnum = programs.Keys.GetEnumerator();
      keyEnum.MoveNext();
      current.Add(keyEnum.Current);


      while (current.Count > 0) {
        foreach (string node in current) {
          if (programs.ContainsKey(node)) {
            programs.Remove(node);
          }
          foreach (string neighbor in graph[node]) {
            if (!seen.Contains(neighbor)) {
              next.Add(neighbor);
            }
          }
        }

        seen.UnionWith(current);
        current = next;
        next = new HashSet<string>();
      }
    }

    Console.WriteLine("Number of groups: " + groups);
  }
}
