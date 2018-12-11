using System;
using System.Collections.Generic;
using System.IO;

public class Advent07 {
  public static void Main(string[] args) {
    var requirements = new Dictionary<char, List<char>>();
    var steps = new HashSet<char>();

    foreach (string line in File.ReadAllLines("input.txt")) {
      string[] components = line.Split(' ');
      char first = components[1][0];
      char second = components[7][0];

      if (!requirements.ContainsKey(second)) {
        requirements.Add(second, new List<char>());
      }

      requirements[second].Add(first);

      steps.Add(first);
      steps.Add(second);
    }

    while (steps.Count > 0) {
      char nextStep = char.MaxValue;
      foreach (char step in steps) {
        if (step < nextStep && (!requirements.ContainsKey(step) || requirements[step].Count == 0)) {
          nextStep = step;
        }
      }

      foreach (KeyValuePair<char, List<char>> keyValuePair in requirements) {
        keyValuePair.Value.Remove(nextStep);
      }

      steps.Remove(nextStep);

      Console.Write(nextStep);
    }
  }
}
