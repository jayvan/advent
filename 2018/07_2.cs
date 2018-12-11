using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    int time = 0;
    int freeElves = 5;

    var completionTime = new Dictionary<char, int>();

    while (steps.Count > 0) {
      var completedSteps = completionTime.Where(a => a.Value == time).Select(a => a.Key).ToArray();
      foreach (char completedStep in completedSteps) {
        completionTime.Remove(completedStep);
      }

      foreach (KeyValuePair<char, List<char>> keyValuePair in requirements) {
        keyValuePair.Value.RemoveAll(a => completedSteps.Contains(a));
      }

      steps.RemoveWhere(a => completedSteps.Contains(a));
      freeElves += completedSteps.Count();

      while (freeElves > 0) {
        char nextStep = char.MaxValue;

        foreach (char step in steps) {
          if (!completionTime.ContainsKey(step) && step < nextStep &&
              (!requirements.ContainsKey(step) || requirements[step].Count == 0)) {
            nextStep = step;
          }
        }

        if (nextStep != char.MaxValue) {
          freeElves--;
          int endTime = time + nextStep - 'A' + 61;
          completionTime.Add(nextStep, endTime);
          Console.WriteLine("Starting step " + nextStep + " at time " + time + " ending at time " + endTime);
        } else {
          break;
        }
      }

      time++;
    }

    Console.WriteLine(time - 1);
  }
}
