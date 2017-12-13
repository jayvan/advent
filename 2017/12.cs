using System;
using System.Collections.Generic;

public class Advent12 {
  public static void Main () {
    var graph = new Dictionary<string, string[]>();
    string line;

    string[] partSplit = new[] { " <-> " };
    string[] neighborSplit = new[] { ", " };

    while ((line = Console.ReadLine()) != null) {
      string[] parts = line.Split(partSplit, StringSplitOptions.RemoveEmptyEntries);
      graph.Add(parts[0], parts[1].Split(neighborSplit, StringSplitOptions.RemoveEmptyEntries));
    }

    var seen = new HashSet<string>();
    var current = new HashSet<string>();
    var next = new HashSet<string>();

    current.Add("0");

    while (current.Count > 0) {
      foreach (string node in current) {
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

    Console.WriteLine("Nodes connected to 0: " + seen.Count);
  }
}
